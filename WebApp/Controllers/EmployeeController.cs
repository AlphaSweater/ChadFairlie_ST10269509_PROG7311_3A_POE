using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Services;
using WebApp.ViewModels.EmployeeViewModels;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	[RoleAuthorize("Employee")]
	public class EmployeeController : BaseController
	{
		private readonly UserSessionService _userSessionService;
		private readonly ProductService _productService;

		public EmployeeController(UserSessionService userSessionService, ProductService productService)
		{
			_userSessionService = userSessionService;
			_productService = productService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> ManageFarmers()
		{
			// TODO: Replace with actual data fetching logic
			var farmers = new List<FarmerViewModel>();
			// Example: farmers = _farmerService.GetAllFarmersForEmployee();

			return View(farmers);
		}

		[HttpGet]
		public async Task<IActionResult> GetFilteredFarmers(string? searchName, string? location, DateTime? createdDate)
		{
			return View();
		}

		public IActionResult AddFarmer()
		{
			return View();
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