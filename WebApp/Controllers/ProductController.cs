using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	public class ProductController : BaseController
	{
		private readonly AuthService _authService;
		private readonly ProductService _productService;

		public ProductController(AuthService userSessionService, ProductService productService)
		{
			_authService = userSessionService;
			_productService = productService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[RoleAuthorize("Farmer")]
		[HttpGet]
		public async Task<IActionResult> AddProduct()
		{
			// Fetch distinct categories from the database
			var categories = await _productService.GetAllCategoriesAsync();
			ViewBag.Categories = categories.Select(c => c.Name).ToList();

			return View();
		}

		[RoleAuthorize("Farmer")]
		[HttpPost]
		public async Task<IActionResult> AddProduct(AddProductViewModel newProductViewModel)
		{
			if (!ModelState.IsValid)
			{
				// Add custom validation errors
				if (string.IsNullOrEmpty(newProductViewModel.Name))
				{
					ModelState.AddModelError("Name", "Product name is required.");
				}

				if (string.IsNullOrEmpty(newProductViewModel.Category))
				{
					ModelState.AddModelError("Category", "Category is required.");
				}

				if (newProductViewModel.Price <= 0)
				{
					ModelState.AddModelError("Price", "Price must be a positive value.");
				}

				// Return the view with validation errors
				var categories = await _productService.GetAllCategoriesAsync();
				ViewBag.Categories = categories.Select(c => c.Name).ToList();
				return View(newProductViewModel);
			}

			newProductViewModel.FarmerId = _authService.GetUserIdRole().Item1;

			// Map ViewModel to Product model
			var newProduct = new Product(newProductViewModel);

			// Handle image upload
			if (newProductViewModel.ImageFile != null)
			{
				using var memoryStream = new MemoryStream();
				await newProductViewModel.ImageFile.CopyToAsync(memoryStream);
				newProduct.Image = memoryStream.ToArray();
			}

			// Save the product
			await _productService.AddProductAsync(newProduct);

			return RedirectToAction("ManageProducts", "Farmer");
		}
	}
}