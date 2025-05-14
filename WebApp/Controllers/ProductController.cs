using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Controllers
{
	/// <summary>
	/// Controller responsible for managing product-related actions, such as adding products.
	/// </summary>
	public class ProductController : BaseController
	{
		private readonly AuthService _authService;
		private readonly ProductService _productService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductController"/> class.
		/// </summary>
		/// <param name="authService">Service for managing user authentication and session.</param>
		/// <param name="productService">Service for managing product operations.</param>
		public ProductController(AuthService authService, ProductService productService)
		{
			_authService = authService;
			_productService = productService;
		}

		/// <summary>
		/// Displays the product dashboard.
		/// </summary>
		/// <returns>The dashboard view.</returns>
		public IActionResult Index() => View();

		/// <summary>
		/// Displays the form to add a new product.
		/// </summary>
		/// <returns>The add product view.</returns>
		[RoleAuthorize("Farmer")]
		[HttpGet]
		public async Task<IActionResult> AddProduct()
		{
			// Retrieve all product categories for selection
			var categories = await _productService.GetAllCategoriesAsync();
			ViewBag.Categories = categories.Select(c => c.Name).ToList();
			return View();
		}

		/// <summary>
		/// Handles the submission of a new product.
		/// </summary>
		/// <param name="newProductViewModel">The view model containing product details.</param>
		/// <returns>Redirects to the farmer's product management page on success, or redisplays the form on failure.</returns>
		[RoleAuthorize("Farmer")]
		[HttpPost]
		public async Task<IActionResult> AddProduct(AddProductViewModel newProductViewModel)
		{
			// Validate the model state
			if (!ModelState.IsValid)
			{
				// Add custom validation errors
				ValidateProductModel(newProductViewModel);

				// Re-fetch categories and return view with errors
				var categories = await _productService.GetAllCategoriesAsync();
				ViewBag.Categories = categories.Select(c => c.Name).ToList();
				return View(newProductViewModel);
			}

			// Map ViewModel to Product model
			var newProduct = new Product(newProductViewModel)
			{
				FarmerId = _authService.GetUserIdRole().Item1 // Set the farmer ID from the session
			};

			// Handle image upload if provided
			await HandleImageUpload(newProductViewModel, newProduct);

			// Save the product to the database
			await _productService.AddAsync(newProduct);

			// Redirect to the farmer's product management page
			return RedirectToAction("ManageProducts", "Farmer");
		}

		/// <summary>
		/// Validates the product model for required fields and constraints.
		/// </summary>
		/// <param name="viewModel">The product view model to validate.</param>
		private void ValidateProductModel(AddProductViewModel viewModel)
		{
			if (string.IsNullOrEmpty(viewModel.Name))
			{
				ModelState.AddModelError("Name", "Product name is required.");
			}

			if (string.IsNullOrEmpty(viewModel.Category))
			{
				ModelState.AddModelError("Category", "Category is required.");
			}

			if (viewModel.Price <= 0)
			{
				ModelState.AddModelError("Price", "Price must be a positive value.");
			}
		}

		/// <summary>
		/// Handles the image upload for a product.
		/// </summary>
		/// <param name="viewModel">The product view model containing the image file.</param>
		/// <param name="product">The product model to update with the image data.</param>
		private async Task HandleImageUpload(AddProductViewModel viewModel, Product product)
		{
			if (viewModel.ImageFile != null)
			{
				// Convert the uploaded image file to a byte array
				using var memoryStream = new MemoryStream();
				await viewModel.ImageFile.CopyToAsync(memoryStream);
				product.Image = memoryStream.ToArray();
			}
		}
	}
}