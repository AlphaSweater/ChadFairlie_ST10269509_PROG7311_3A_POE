using WebApp.Models;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.ViewModels.EmployeeViewModels
{
	public class DetailedFarmerViewModel
	{
		public int FarmerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName => $"{FirstName} {LastName}";
		public string Email { get; set; }
		public DateTime? CreatedOn { get; set; }
		public EmployeeViewModel CreatedBy { get; set; }
		public List<ProductViewModel> Products { get; set; }

		public DetailedFarmerViewModel()
		{
			// Default constructor
		}

		// Constructor to convert Model to ViewModel
		public DetailedFarmerViewModel(Farmer farmer, List<Product> products)
		{
			FarmerId = farmer.FarmerId;
			FirstName = farmer.FirstName;
			LastName = farmer.LastName;
			Email = farmer.Email;
			CreatedOn = farmer.CreatedOn;
			CreatedBy = new EmployeeViewModel(farmer.CreatedByEmployee); // Assuming CreatedByEmployee is a navigation property
			Products = products.Select(p => new ProductViewModel(p)).ToList(); // Convert each product to ProductViewModel
		}
	}
}