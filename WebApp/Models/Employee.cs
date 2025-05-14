using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
	[Table("TBL_Employees")] // Maps the class to the table
	public class Employee : BaseEntity
	{
		[Key]
		[Column("employee_id")]
		public int EmployeeId { get; set; } // Maps to employee_id (Primary Key)

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

		public string FullName => $"{FirstName} {LastName}";
	}
}