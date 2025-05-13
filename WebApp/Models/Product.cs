using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
	}
}