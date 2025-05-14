using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
	public class AgriDbContext : DbContext
	{
		public AgriDbContext(DbContextOptions<AgriDbContext> options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Farmer> Farmers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Farmer → Employee (Created By)
			modelBuilder.Entity<Farmer>()
				.HasOne(f => f.CreatedByEmployee)
				.WithMany()
				.HasForeignKey(f => f.CreatedByEmployeeId);

			// Product → Farmer
			modelBuilder.Entity<Product>()
				.HasOne(p => p.Farmer)
				.WithMany()
				.HasForeignKey(p => p.FarmerId);

			// Configure Categories table
			modelBuilder.Entity<Category>()
				.Property(c => c.CreatedOn)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
		}
	}
}