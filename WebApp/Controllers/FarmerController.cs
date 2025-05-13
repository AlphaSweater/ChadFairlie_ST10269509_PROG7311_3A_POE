using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.ViewModels.Farmer;

namespace WebApp.Controllers
{
	[RoleAuthorize("Farmer")]
	public class FarmerController : BaseController
	{
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> ManageProducts()
		{
			// Fetch the initial list of products
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

			return Json(allProducts);
		}

		private Task<IEnumerable<FarmerProductCardViewModel>> GetFarmerProductsAsync()
		{
			// Simulate fetching data (replace with actual database/service logic)
			var products = new List<FarmerProductCardViewModel>
			{
				new FarmerProductCardViewModel
				{
					ProductId = 1,
					Name = "Tomatoes",
					Category = "Vegetables",
					Price = 20.5,
					CreatedOn = DateTime.Now.AddDays(-10)
				},
				new FarmerProductCardViewModel
				{
					ProductId = 2,
					Name = "Apples",
					Category = "Fruits",
					Price = 15.0,
					CreatedOn = DateTime.Now.AddDays(-5)
				}
			};

			return Task.FromResult(products.AsEnumerable());
		}
	}
}