namespace WebApp.ViewModels.Employee
{
	public class FarmerViewModel
	{
		public int FarmerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName => $"{FirstName} {LastName}";
		public string Email { get; set; }
		public DateTime? CreatedOn { get; set; }
	}
}