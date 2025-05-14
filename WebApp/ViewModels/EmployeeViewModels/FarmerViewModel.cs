using WebApp.Models;

namespace WebApp.ViewModels.EmployeeViewModels
{
	public class FarmerViewModel
	{
		public int FarmerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName => $"{FirstName} {LastName}";
		public string Email { get; set; }
		public DateTime? CreatedOn { get; set; }
		public EmployeeViewModel CreatedBy { get; set; }

		public FarmerViewModel()
		{
			// Default constructor
		}

		// Constructor to convert Model to ViewModel
		public FarmerViewModel(Farmer farmer)
		{
			FarmerId = farmer.FarmerId;
			FirstName = farmer.FirstName;
			LastName = farmer.LastName;
			Email = farmer.Email;
			CreatedOn = farmer.CreatedOn;
			CreatedBy = new EmployeeViewModel(farmer.CreatedByEmployee); // Assuming CreatedByEmployee is a navigation property
		}
	}
}