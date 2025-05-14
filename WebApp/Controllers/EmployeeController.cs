using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels.EmployeeViewModels;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	/// <summary>
	/// Controller responsible for managing employee-related actions, including managing farmers and products.
	/// </summary>
	[RoleAuthorize("Employee")]
	public class EmployeeController : BaseController
	{
		private readonly AuthService _authService;
		private readonly ProductService _productService;
		private readonly FarmerService _farmerService;

		/// <summary>
		/// Initializes a new instance of the <see cref="EmployeeController"/> class.
		/// </summary>
		/// <param name="authService">Service for authentication-related operations.</param>
		/// <param name="productService">Service for managing products.</param>
		/// <param name="farmerService">Service for managing farmers.</param>
		public EmployeeController(AuthService authService, ProductService productService, FarmerService farmerService)
		{
			_authService = authService;
			_productService = productService;
			_farmerService = farmerService;
		}

		/// <summary>
		/// Displays the employee dashboard.
		/// </summary>
		/// <returns>The dashboard view.</returns>
		public IActionResult Index() => View();

		/// <summary>
		/// Displays a list of all farmers.
		/// </summary>
		/// <returns>The view with a list of farmers.</returns>
		public async Task<IActionResult> ManageFarmers()
		{
			var farmers = await _farmerService.GetAllFarmersAsync();
			var viewModels = farmers.Select(f => new FarmerViewModel(f));
			return View(viewModels);
		}

		/// <summary>
		/// Filters farmers based on search criteria.
		/// </summary>
		/// <param name="searchName">The name to search for.</param>
		/// <param name="createdByMe">Whether to filter by farmers created by the current employee.</param>
		/// <returns>A partial view with the filtered list of farmers.</returns>
		[HttpGet]
		public async Task<IActionResult> GetFilteredFarmers(string? searchName, string? createdByMe)
		{
			var employeeId = _authService.GetUserIdRole().Item1;
			var farmers = (createdByMe == "on")
				? await _farmerService.FilterFarmersAsync(null, employeeId)
				: await _farmerService.GetAllFarmersAsync();

			if (!string.IsNullOrEmpty(searchName))
			{
				searchName = searchName.Trim();
				farmers = farmers.Where(f =>
					f.FirstName.Contains(searchName, StringComparison.OrdinalIgnoreCase) ||
					f.LastName.Contains(searchName, StringComparison.OrdinalIgnoreCase) ||
					f.Email.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			var viewModels = farmers.Select(f => new FarmerViewModel(f));
			return PartialView("_FarmerCardList", viewModels);
		}

		/// <summary>
		/// Displays detailed information about a specific farmer.
		/// </summary>
		/// <param name="farmerId">The ID of the farmer to view.</param>
		/// <returns>The view with farmer details and their products.</returns>
		public async Task<IActionResult> ViewFarmer(int farmerId)
		{
			var farmer = await _farmerService.GetFarmerByIdAsync(farmerId);
			if (farmer == null)
				return NotFound();

			var products = await _productService.GetAllProductsByFarmerIdAsync(farmerId);
			var categories = await _productService.GetAllCategoriesAsync();
			ViewBag.Categories = categories.Select(c => c.Name).ToList();

			return View(new DetailedFarmerViewModel(farmer, products));
		}

		/// <summary>
		/// Displays the form to add a new farmer.
		/// </summary>
		/// <returns>The add farmer view.</returns>
		[HttpGet]
		public IActionResult AddFarmer() => View();

		/// <summary>
		/// Handles the submission of a new farmer.
		/// </summary>
		/// <param name="model">The view model containing farmer details.</param>
		/// <returns>Redirects to the farmer management page on success, or redisplays the form on failure.</returns>
		[HttpPost]
		public async Task<IActionResult> AddFarmer(AddFarmerViewModel model)
		{
			ValidateFarmerModel(model);

			if (!ModelState.IsValid)
				return View(model);

			if (await _authService.IsEmailInUseAsync(model.Email))
			{
				ModelState.AddModelError("Email", "Email is already in use.");
				return View(model);
			}

			model.CreatedByEmployeeId = _authService.GetUserIdRole().Item1;
			model.HashPassword = _authService.HashPassword(model.Password);

			await _farmerService.AddFarmerAsync(new Farmer(model));
			return RedirectToAction(nameof(ManageFarmers));
		}

		/// <summary>
		/// Displays the form to edit an existing farmer.
		/// </summary>
		/// <param name="farmerId">The ID of the farmer to edit.</param>
		/// <returns>The edit farmer view.</returns>
		public IActionResult EditFarmer(int farmerId)
		{
			var farmer = _farmerService.GetFarmerByIdAsync(farmerId).Result;
			if (farmer == null)
				return NotFound();

			return View(new AddFarmerViewModel(farmer));
		}

		/// <summary>
		/// Handles the submission of updated farmer details.
		/// </summary>
		/// <param name="model">The view model containing updated farmer details.</param>
		/// <returns>Redirects to the farmer management page on success, or redisplays the form on failure.</returns>
		[HttpPost]
		public async Task<IActionResult> UpdateFarmer(AddFarmerViewModel model)
		{
			ValidateFarmerModel(model, isUpdate: true);

			if (!ModelState.IsValid)
				return View("EditFarmer", model);

			var farmer = await _farmerService.GetFarmerByIdAsync(model.FarmerId);
			if (farmer != null)
			{
				farmer.FirstName = model.FirstName;
				farmer.LastName = model.LastName;
				farmer.Email = model.Email;
				farmer.UpdatedOn = DateTime.UtcNow;

				await _farmerService.UpdateFarmerAsync(farmer);
			}
			return RedirectToAction(nameof(ManageFarmers));
		}

		/// <summary>
		/// Deletes a farmer by their ID.
		/// </summary>
		/// <param name="farmerId">The ID of the farmer to delete.</param>
		/// <returns>Redirects to the farmer management page.</returns>
		[HttpPost]
		public async Task<IActionResult> DeleteFarmer(int farmerId)
		{
			var farmer = await _farmerService.GetFarmerByIdAsync(farmerId);
			if (farmer != null)
			{
				await _farmerService.DeleteFarmerAsync(farmer.FarmerId);
			}
			return RedirectToAction(nameof(ManageFarmers));
		}

		/// <summary>
		/// Displays a list of all products.
		/// </summary>
		/// <returns>The view with a list of products.</returns>
		public async Task<IActionResult> ManageProducts()
		{
			var products = await _productService.GetAllProductsAsync();
			var categories = await _productService.GetAllCategoriesAsync();
			ViewBag.Categories = categories.Select(c => c.Name).ToList();

			return View(products.Select(p => new ProductViewModel(p)));
		}

		/// <summary>
		/// Filters products based on search criteria.
		/// </summary>
		/// <param name="searchName">The product name to search for.</param>
		/// <param name="farmerName">The farmer's name to filter by.</param>
		/// <param name="category">The category to filter by.</param>
		/// <param name="createdDate">The creation date to filter by.</param>
		/// <returns>A partial view with the filtered list of products.</returns>
		[HttpGet]
		public async Task<IActionResult> GetFilteredProducts(string? searchName, string? farmerName, string? category, DateTime? createdDate)
		{
			var products = (await _productService.GetAllProductsAsync()).Select(p => new ProductViewModel(p));

			if (!string.IsNullOrWhiteSpace(searchName))
				products = products.Where(p => p.Name.Contains(searchName.Trim(), StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(farmerName))
				products = products.Where(p => p.CreatedBy.FullName.Contains(farmerName.Trim(), StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(category))
				products = products.Where(p => p.Category.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase));

			if (createdDate.HasValue)
				products = products.Where(p => p.CreatedOn?.Date == createdDate.Value.Date);

			return PartialView("_ProductCardList", products);
		}

		/// <summary>
		/// Filters products by farmer and additional criteria.
		/// </summary>
		/// <param name="farmerId">The ID of the farmer to filter by.</param>
		/// <param name="category">The category to filter by.</param>
		/// <param name="startDate">The start date for filtering.</param>
		/// <param name="endDate">The end date for filtering.</param>
		/// <returns>A partial view with the filtered list of products.</returns>
		[HttpGet]
		public async Task<IActionResult> GetFilteredProductsByFarmer(int farmerId, string? category, DateTime? startDate, DateTime? endDate)
		{
			var products = await _productService.GetAllProductsByFarmerIdAsync(farmerId);

			if (!string.IsNullOrWhiteSpace(category))
				products = products.Where(p => p.Category.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();

			if (startDate.HasValue)
				products = products.Where(p => p.CreatedOn >= startDate.Value).ToList();

			if (endDate.HasValue)
				products = products.Where(p => p.CreatedOn <= endDate.Value).ToList();

			return PartialView("_ProductCardList", products.Select(p => new ProductViewModel(p)));
		}

		/// <summary>
		/// Validates the farmer model for required fields and constraints.
		/// </summary>
		/// <param name="model">The farmer model to validate.</param>
		/// <param name="isUpdate">Indicates whether this is an update operation.</param>
		private void ValidateFarmerModel(AddFarmerViewModel model, bool isUpdate = false)
		{
			if (string.IsNullOrWhiteSpace(model.FirstName))
				ModelState.AddModelError("FirstName", "First name is required.");

			if (string.IsNullOrWhiteSpace(model.LastName))
				ModelState.AddModelError("LastName", "Last name is required.");

			if (string.IsNullOrWhiteSpace(model.Email))
				ModelState.AddModelError("Email", "Email is required.");
			else if (!model.Email.Contains("@"))
				ModelState.AddModelError("Email", "Invalid email address.");

			if (!isUpdate)
			{
				if (model.Password != model.ConfirmPassword)
					ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
			}
		}
	}
}