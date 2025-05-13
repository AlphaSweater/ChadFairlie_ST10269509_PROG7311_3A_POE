namespace WebApp.ViewModels.Farmer
{
	public class FarmerProductCardViewModel
	{
		public int ProductId { get; set; } // Product ID
		public string Name { get; set; } // Product Name
		public string Category { get; set; } // Product Category
		public double Price { get; set; } // Product Price
		public byte[]? Image { get; set; } // Product Image (optional)
		public DateTime? CreatedOn { get; set; } // Product Creation Date
	}
}