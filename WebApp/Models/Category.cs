using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
	[Table("TBL_Categories")] // Maps the class to the table
	public class Category : BaseEntity
	{
		[Key]
		[Column("category_id")]
		public int CategoryId { get; set; } // Primary Key

		[Required]
		[MaxLength(100)]
		[Column("name")]
		public string Name { get; set; } // Category name
	}
}