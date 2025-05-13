using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.ViewModels.Product;

namespace WebApp.Controllers
{
	public class ProductController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}

		[RoleAuthorize("Farmer")]
		[HttpGet]
		public async Task<IActionResult> AddProduct()
		{
			return View();
		}

		[RoleAuthorize("Farmer")]
		[HttpPost]
		public async Task<IActionResult> AddProduct(AddProductViewModel newProductViewModel)
		{
			return RedirectToAction("ManageProducts", "Farmer");
		}
	}
}