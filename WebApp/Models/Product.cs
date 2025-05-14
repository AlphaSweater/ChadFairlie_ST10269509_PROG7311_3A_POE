using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.ViewModels.ProductViewModels;

namespace WebApp.Models
{
	[Table("TBL_Products")] // Maps the class to the table
	public class Product
	{
		[Key]
		[Column("product_id")]
		public int ProductId { get; set; } // Maps to product_id (Primary Key)

		[Required]
		[MaxLength(100)]
		[Column("name")]
		public string Name { get; set; } // Maps to name

		[Required]
		[MaxLength(100)]
		[Column("category")]
		public string Category { get; set; } // Maps to category

		[Required]
		[Column("price")]
		public double Price { get; set; } // Maps to price (REAL in SQLite)

		[Column("image")]
		public byte[]? Image { get; set; } // Maps to image (BLOB)

		[Required]
		[Column("farmer_id")]
		public int FarmerId { get; set; } // Maps to farmer_id (Foreign Key)

		[Column("created_on")]
		public DateTime? CreatedOn { get; set; } // Maps to created_on (TEXT)

		[Column("updated_on")]
		public DateTime? UpdatedOn { get; set; } // Maps to updated_on (TEXT)

		[Required]
		[Column("is_deleted")]
		public bool IsDeleted { get; set; } = false; // Maps to is_deleted (INTEGER)

		// Navigation Property
		[ForeignKey("FarmerId")]
		public Farmer Farmer { get; set; }

		// Constructor
		public Product()
		{
			// Default constructor
		}

		// Constructor to convert ViewModel to Model
		public Product(AddProductViewModel newProductViewModel)
		{
			Name = newProductViewModel.Name;
			Category = newProductViewModel.Category;
			Price = (double)newProductViewModel.Price;
			// Convert IFormFile to byte array
			if (newProductViewModel.ImageFile != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					newProductViewModel.ImageFile.CopyTo(memoryStream);
					Image = memoryStream.ToArray();
				}
			}
			FarmerId = newProductViewModel.FarmerId; // Assuming FarmerId is passed in the ViewModel
			CreatedOn = DateTime.UtcNow; // Set CreatedOn to current UTC time
		}
	}
}