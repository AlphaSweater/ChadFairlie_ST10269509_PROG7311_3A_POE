using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.ViewModels.EmployeeViewModels;

namespace WebApp.Models
{
	[Table("TBL_Farmers")] // Maps the class to the table
	public class Farmer : BaseEntity
	{
		[Key]
		[Column("farmer_id")]
		public int FarmerId { get; set; } // Maps to farmer_id (Primary Key)

		[Required]
		[MaxLength(55)]
		[Column("first_name")]
		public string FirstName { get; set; } // Maps to first_name

		[Required]
		[MaxLength(55)]
		[Column("last_name")]
		public string LastName { get; set; } // Maps to last_name

		[Required]
		[MaxLength(255)]
		[Column("email")]
		[EmailAddress]
		public string Email { get; set; } // Maps to email

		[Required]
		[Column("password_hash")]
		public string PasswordHash { get; set; } // Maps to password_hash

		[Required]
		[Column("created_by_employee_id")]
		public int CreatedByEmployeeId { get; set; } // Maps to created_by_employee_id (Foreign Key)

		// Navigation Property
		[ForeignKey("CreatedByEmployeeId")]
		public Employee CreatedByEmployee { get; set; }

		public string FullName => $"{FirstName} {LastName}";

		// Constructor
		public Farmer()
		{
			// Default constructor
		}

		// Constructor to convert ViewModel to Model
		public Farmer(AddFarmerViewModel newFarmerViewModel)
		{
			FirstName = newFarmerViewModel.FirstName;
			LastName = newFarmerViewModel.LastName;
			Email = newFarmerViewModel.Email;
			PasswordHash = newFarmerViewModel.HashPassword;
			CreatedByEmployeeId = newFarmerViewModel.CreatedByEmployeeId;
		}
	}
}