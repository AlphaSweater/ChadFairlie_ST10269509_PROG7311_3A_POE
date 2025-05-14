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
		private readonly AuthService _userSessionService;
		private readonly ProductService _productService;
		private readonly FarmerService _farmerService;

		public EmployeeController(AuthService userSessionService, ProductService productService, FarmerService farmerService)
		{
			_userSessionService = userSessionService;
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