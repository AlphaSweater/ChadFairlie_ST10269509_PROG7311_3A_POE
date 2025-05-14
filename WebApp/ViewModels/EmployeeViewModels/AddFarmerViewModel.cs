using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels.EmployeeViewModels
{
	public class AddFarmerViewModel
	{
		public int FarmerId { get; set; } = 0; // No validation applied

		[Required, MaxLength(55)]
		public string FirstName { get; set; }

		[Required, MaxLength(55)]
		public string LastName { get; set; }

		[Required, EmailAddress, MaxLength(255)]
		public string Email { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		public string? HashPassword { get; set; }

		[Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		public DateTime? CreatedOn { get; set; } = DateTime.Now;

		public int CreatedByEmployeeId { get; set; } = 1;

		// Constructor to convert Model to ViewModel
		public AddFarmerViewModel(Farmer farmer)
		{
			FarmerId = farmer.FarmerId;
			FirstName = farmer.FirstName;
			LastName = farmer.LastName;
			Email = farmer.Email;
			CreatedOn = farmer.CreatedOn;
			CreatedByEmployeeId = farmer.CreatedByEmployeeId;
		}

		// Default constructor
		public AddFarmerViewModel()
		{
			// Default constructor
		}
	}
}