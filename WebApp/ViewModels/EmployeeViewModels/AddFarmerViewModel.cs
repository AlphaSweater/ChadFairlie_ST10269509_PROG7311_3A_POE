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

		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public int CreatedByEmployeeId { get; set; } // set in the controller/server
	}
}