using WebApp.Models;

namespace WebApp.ViewModels.EmployeeViewModels
{
	public class EmployeeViewModel
	{
		public int EmployeeId { get; set; } // Employee ID
		public string FirstName { get; set; } // Employee First Name
		public string LastName { get; set; } // Employee Last Name
		public string FullName => $"{FirstName} {LastName}"; // Full Name property
		public string Email { get; set; } // Employee Email
		public DateTime? CreatedOn { get; set; } // Account Creation Date

		public EmployeeViewModel()
		{
			// Default constructor
		}

		// Constructor to convert Model to ViewModel
		public EmployeeViewModel(Employee employee)
		{
			EmployeeId = employee.EmployeeId;
			FirstName = employee.FirstName;
			LastName = employee.LastName;
			Email = employee.Email;
			CreatedOn = employee.CreatedOn;
		}
	}
}