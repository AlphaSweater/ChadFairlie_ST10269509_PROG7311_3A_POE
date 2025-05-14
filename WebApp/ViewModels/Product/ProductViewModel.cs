using WebApp.ViewModels.Employee;

namespace WebApp.ViewModels.Product
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
	}
}