using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Services;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	/// <summary>
	/// Controller responsible for managing farmer-related actions, including managing products.
	/// </summary>
	[RoleAuthorize("Farmer")]
	public class FarmerController : BaseController
	{
		private readonly AuthService _authService;
		private readonly ProductService _productService;

		/// <summary>
		/// Initializes a new instance of the <see cref="FarmerController"/> class.
		/// </summary>
		/// <param name="userSessionService">Service for managing user session and authentication.</param>
		/// <param name="productService">Service for managing products.</param>
		public FarmerController(AuthService userSessionService, ProductService productService)
		{
			_authService = userSessionService;
			_productService = productService;
		}

		/// <summary>
		/// Displays the farmer dashboard.
		/// </summary>
		/// <returns>The dashboard view.</returns>
		public IActionResult Index() => View();

		/// <summary>
		/// Displays a list of all products created by the current farmer.
		/// </summary>
		/// <returns>The view with a list of products.</returns>
		public async Task<IActionResult> ManageProducts()
		{
			// Get the current farmer's ID from the session
			var farmerId = _authService.GetUserIdRole().Item1;

			// Retrieve all products created by the farmer
			var products = await _productService.GetAllProductsByFarmerIdAsync(farmerId);
			var viewModels = products.Select(p => new ProductViewModel(p));

			// Retrieve all product categories for filtering
			var categories = await _productService.GetAllCategoriesAsync();
			ViewBag.Categories = categories.Select(c => c.Name).ToList();

			return View(viewModels);
		}

		/// <summary>
		/// Filters products created by the current farmer based on search criteria.
		/// </summary>
		/// <param name="searchName">The product name to search for.</param>
		/// <param name="category">The category to filter by.</param>
		/// <param name="createdDate">The creation date to filter by.</param>
		/// <returns>A partial view with the filtered list of products.</returns>
		[HttpGet]
		public async Task<IActionResult> GetFilteredProducts(string? searchName, string? category, DateTime? createdDate)
		{
			// Get the current farmer's ID from the session
			var farmerId = _authService.GetUserIdRole().Item1;

			// Retrieve all products created by the farmer
			var products = await _productService.GetAllProductsByFarmerIdAsync(farmerId);
			var viewModels = products.Select(p => new ProductViewModel(p));

			// Apply filters based on the provided criteria
			if (!string.IsNullOrWhiteSpace(searchName))
				viewModels = viewModels.Where(p => p.Name.Contains(searchName.Trim(), StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(category))
				viewModels = viewModels.Where(p => p.Category.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase));

			if (createdDate.HasValue)
				viewModels = viewModels.Where(p => p.CreatedOn?.Date == createdDate.Value.Date);

			return PartialView("_ProductCardList", viewModels);
		}
	}
}