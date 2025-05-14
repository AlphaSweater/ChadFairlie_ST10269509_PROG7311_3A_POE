using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Services;
using WebApp.ViewModels.Employee;
using WebApp.ViewModels.Product;

namespace WebApp.Controllers
{
	[RoleAuthorize("Farmer")]
	public class FarmerController : BaseController
	{
		private readonly UserSessionService _userSessionService;

		public FarmerController(UserSessionService userSessionService)
		{
			_userSessionService = userSessionService;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> ManageProducts()
		{
			var allProducts = await GetFarmerProductsAsync();
			return View(allProducts);
		}

		[HttpGet]
		public async Task<IActionResult> GetFilteredProducts(string? searchName, string? category, DateTime? createdDate)
		{
			var allProducts = await GetFarmerProductsAsync();

			// Apply filters
			if (!string.IsNullOrEmpty(searchName))
			{
				allProducts = allProducts.Where(p => p.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(category))
			{
				allProducts = allProducts.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
			}

			if (createdDate.HasValue)
			{
				allProducts = allProducts.Where(p => p.CreatedOn?.Date == createdDate.Value.Date);
			}

			return PartialView("_ProductCardList", allProducts);
		}

		private Task<IEnumerable<ProductViewModel>> GetFarmerProductsAsync()
		{
			// Simulate fetching data (replace with actual database/service logic)
			var products = new List<ProductViewModel>
			{
				new ProductViewModel
				{
					ProductId = 1,
					Name = "Tomatoes",
					Category = "Vegetables",
					Price = 20.5,
					CreatedBy = new FarmerViewModel {
						FarmerId = 2,
						FirstName = "John",
						LastName = "Doe",
						Email = "John@farmer.co.za"
					},
					CreatedOn = DateTime.Now.AddDays(-10)
				},
				new ProductViewModel
				{
					ProductId = 2,
					Name = "Apples",
					Category = "Fruits",
					Price = 15.0,
					CreatedBy = new FarmerViewModel {
						FarmerId = 2,
						FirstName = "John",
						LastName = "Doe",
						Email = "John@farmer.co.za"
					},
					CreatedOn = DateTime.Now.AddDays(-5)
				}
			};

			return Task.FromResult(products.AsEnumerable());
		}
	}
}