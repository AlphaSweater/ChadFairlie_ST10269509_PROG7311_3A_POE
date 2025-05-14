using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Services;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	[RoleAuthorize("Farmer")]
	public class FarmerController : BaseController
	{
		private readonly UserSessionService _userSessionService;
		private readonly ProductService _productService;

		public FarmerController(UserSessionService userSessionService, ProductService productService)
		{
			_userSessionService = userSessionService;
			_productService = productService;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> ManageProducts()
		{
			// Filter products by the logged-in farmer's ID
			var farmerId = _userSessionService.GetUserIdRole().Item1;
			var allModelProducts = await _productService.GetAllProductsByFarmerIdAsync(farmerId);

			// Convert Models to viewModels
			var allProductsViewModels = allModelProducts.Select(p => new ProductViewModel(p));

			return View(allProductsViewModels);
		}

		[HttpGet]
		public async Task<IActionResult> GetFilteredProducts(string? searchName, string? category, DateTime? createdDate)
		{
			var farmerId = _userSessionService.GetUserIdRole().Item1;
			var allModelProducts = await _productService.GetAllProductsByFarmerIdAsync(farmerId);

			// Convert Models to viewModels
			var allProductsViewModels = allModelProducts.Select(p => new ProductViewModel(p));

			// Apply filters
			if (!string.IsNullOrEmpty(searchName))
			{
				allProductsViewModels = allProductsViewModels.Where(p => p.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
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