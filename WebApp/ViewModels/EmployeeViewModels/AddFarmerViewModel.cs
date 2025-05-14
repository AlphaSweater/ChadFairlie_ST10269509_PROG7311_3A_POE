using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.EmployeeViewModels
{
	public class AddFarmerViewModel
	{
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

		public DateTime CreatedOn { get; set; } = DateTime.Now; // No validation applied

		public int CreatedByEmployeeId { get; set; } = 1;// set in the controller/server
	}
}