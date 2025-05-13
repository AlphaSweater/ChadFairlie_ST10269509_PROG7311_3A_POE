using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
	[Table("TBL_Farmers")] // Maps the class to the table
	public class Farmer
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
		public string Email { get; set; } // Maps to email

		[Required]
		[Column("password_hash")]
		public string PasswordHash { get; set; } // Maps to password_hash

		[Column("created_on")]
		public DateTime? CreatedOn { get; set; } // Maps to created_on (TEXT)

		[Column("updated_on")]
		public DateTime? UpdatedOn { get; set; } // Maps to updated_on (TEXT)

		[Required]
		[Column("is_deleted")]
		public bool IsDeleted { get; set; } = false; // Maps to is_deleted (INTEGER)

		[Required]
		[Column("created_by_employee_id")]
		public int CreatedByEmployeeId { get; set; } // Maps to created_by_employee_id (Foreign Key)

		// Navigation Property
		[ForeignKey("CreatedByEmployeeId")]
		public Employee CreatedByEmployee { get; set; }
	}
}