using WebApp.Models;
using WebApp.ViewModels.EmployeeViewModels;

namespace WebApp.ViewModels.ProductViewModels
{
	public class ProductViewModel
	{
		public int ProductId { get; set; } // Product ID
		public string Name { get; set; } // Product Name
		public string Category { get; set; } // Product Category
		public double Price { get; set; } // Product Price
		public byte[]? Image { get; set; } // Product Image (optional)
		public FarmerViewModel? CreatedBy { get; set; } // Farmer who created the product
		public DateTime? CreatedOn { get; set; } // Product Creation Date

		public ProductViewModel()
		{
			// Default constructor
		}

		// Constructor to convert Model to ViewModel
		public ProductViewModel(Product product)
		{
			ProductId = product.ProductId;
			Name = product.Name;
			Category = product.Category;
			Price = product.Price;
			Image = product.Image;
			CreatedBy = new FarmerViewModel(product.Farmer); // Assuming Farmer is a navigation property
			CreatedOn = product.CreatedOn;
		}


	}
}