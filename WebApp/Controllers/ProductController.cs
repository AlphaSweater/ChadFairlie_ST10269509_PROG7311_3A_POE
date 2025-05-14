using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Services;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	public class ProductController : BaseController
	{
		private readonly AuthService _userSessionService;

		public ProductController(AuthService userSessionService)
		{
			_userSessionService = userSessionService;
		}

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