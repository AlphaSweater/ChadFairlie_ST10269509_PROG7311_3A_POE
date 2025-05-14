using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.ProductViewModels
{
	public class AddProductViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Category { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
		public decimal Price { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.Today;

		public IFormFile ImageFile { get; set; }

		public int FarmerId { get; set; }
	}
}