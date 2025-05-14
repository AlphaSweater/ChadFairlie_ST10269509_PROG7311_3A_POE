using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels.EmployeeViewModels;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	[RoleAuthorize("Employee")]
	public class EmployeeController : BaseController
	{
		private readonly AuthService _authService;
		private readonly ProductService _productService;
		private readonly FarmerService _farmerService;

		public EmployeeController(AuthService userSessionService, ProductService productService, FarmerService farmerService)
		{
			_authService = userSessionService;
			_productService = productService;
			_farmerService = farmerService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> ManageFarmers()
		{
			var allModelFarmers = await _farmerService.GetAllFarmersAsync();

			var allFarmersViewModels = allModelFarmers.Select(f => new FarmerViewModel(f));

			return View(allFarmersViewModels);
		}

		[HttpGet]
		public async Task<IActionResult> GetFilteredFarmers(string? searchName)
		{
			var allModelFarmers = await _farmerService.GetAllFarmersAsync();

			// Apply filters
			if (!string.IsNullOrEmpty(searchName))
			{
				allModelFarmers = allModelFarmers.Where(f =>
					f.FirstName.Contains(searchName, StringComparison.OrdinalIgnoreCase) ||
					f.LastName.Contains(searchName, StringComparison.OrdinalIgnoreCase) ||
					f.Email.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			var farmerViewModels = allModelFarmers.Select(f => new FarmerViewModel(f));

			return PartialView("_FarmerCardList", farmerViewModels);
		}

		[HttpGet]
		public IActionResult AddFarmer()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddFarmer(AddFarmerViewModel newFarmerViewModel)
		{
			if (!ModelState.IsValid)
			{
				// If the model state is invalid, return the view with the current model to show validation errors
				if (string.IsNullOrEmpty(newFarmerViewModel.FirstName))
				{
					ModelState.AddModelError("FirstName", "First name is required.");
				}

				if (string.IsNullOrEmpty(newFarmerViewModel.LastName))
				{
					ModelState.AddModelError("LastName", "Last name is required.");
				}

				if (string.IsNullOrEmpty(newFarmerViewModel.Email))
				{
					ModelState.AddModelError("Email", "Email is required.");
				}
				else if (!newFarmerViewModel.Email.Contains("@"))
				{
					ModelState.AddModelError("Email", "Invalid email address.");
				}

				if (newFarmerViewModel.Password != newFarmerViewModel.ConfirmPassword)
				{
					ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
				}

				return View(newFarmerViewModel);
			}

			// Check if the email is already in use
			var existingUser = await _authService.IsEmailInUseAsync(newFarmerViewModel.Email);

			if (existingUser)
			{
				ModelState.AddModelError("Email", "Email is already in use.");
				return View(newFarmerViewModel);
			}

			newFarmerViewModel.CreatedByEmployeeId = _authService.GetUserIdRole().Item1;

			newFarmerViewModel.HashPassword = _authService.HashPassword(newFarmerViewModel.Password);

			var newFarmer = new Farmer(newFarmerViewModel);

			await _farmerService.AddFarmerAsync(newFarmer);

			return RedirectToAction("ManageFarmers", "Employee");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteFarmer(int farmerId)
		{
			var farmer = await _farmerService.GetFarmerByIdAsync(farmerId);
			if (farmer != null)
			{
				await _farmerService.DeleteFarmerAsync(farmer);
			}
			return RedirectToAction("ManageFarmers", "Employee");
		}

		public async Task<IActionResult> ManageProducts()
		{
			var allModelProducts = await _productService.GetAllProductsAsync();

			var allProductsViewModels = allModelProducts.Select(p => new ProductViewModel(p));

			return View(allProductsViewModels);
		}

		[HttpGet]
		public async Task<IActionResult> GetFilteredProducts(string? searchName, string? farmerName, string? category, DateTime? createdDate)
		{
			var allModelProducts = await _productService.GetAllProductsAsync();

			var allProductsViewModels = allModelProducts.Select(p => new ProductViewModel(p));

			// Apply filters
			if (!string.IsNullOrEmpty(searchName))
			{
				allProductsViewModels = allProductsViewModels.Where(p => p.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(farmerName))
			{
				allProductsViewModels = allProductsViewModels.Where(p => p.CreatedBy.FullName.Contains(farmerName, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(category))
			{
				allProductsViewModels = allProductsViewModels.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
			}

			if (createdDate.HasValue)
			{
				allProductsViewModels = allProductsViewModels.Where(p => p.CreatedOn?.Date == createdDate.Value.Date);
			}

			return PartialView("_ProductCardList", allProductsViewModels);
		}
	}
}