using WebApp.Models;
using WebApp.Services;

namespace WebApp.Data
{
	public static class DatabaseSeeder
	{
		public static void ClearDatabase(AgriDbContext context)
		{
			// Remove all records from each table
			context.Products.RemoveRange(context.Products);
			context.Farmers.RemoveRange(context.Farmers);
			context.Employees.RemoveRange(context.Employees);

			// Save changes to apply deletions
			context.SaveChanges();
		}

		public static void Seed(AgriDbContext context, AuthService authService)
		{
			// Ensure the database is created
			context.Database.EnsureCreated();

			// Helper method to load image as byte array
			byte[]? LoadImage(string productName)
			{
				string imagePath = Path.Combine("wwwroot", "image", "seeds", $"{productName.ToLower()}.jpg");
				if (File.Exists(imagePath))
				{
					return File.ReadAllBytes(imagePath);
				}
				return null;
			}

			// Seed Employees
			if (!context.Employees.Any())
			{
				context.Employees.AddRange(
					new Employee
					{
						EmployeeId = 1,
						FirstName = "Admin",
						LastName = "Guy",
						Email = "admin@gmail.com",
						PasswordHash = authService.HashPassword("admin"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Employee
					{
						EmployeeId = 2,
						FirstName = "Jane",
						LastName = "Smith",
						Email = "jane.smith@example.com",
						PasswordHash = authService.HashPassword("password2"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					}
				);
			}

			// Seed Farmers
			if (!context.Farmers.Any())
			{
				context.Farmers.AddRange(
					new Farmer
					{
						FarmerId = 1,
						FirstName = "Alice",
						LastName = "Brown",
						Email = "alice.brown@example.com",
						PasswordHash = authService.HashPassword("password3"),
						CreatedByEmployeeId = 1,
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Farmer
					{
						FarmerId = 2,
						FirstName = "Bob",
						LastName = "Green",
						Email = "bob.green@example.com",
						PasswordHash = authService.HashPassword("password4"),
						CreatedByEmployeeId = 1,
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Farmer
					{
						FarmerId = 3,
						FirstName = "Charlie",
						LastName = "White",
						Email = "charlie.white@example.com",
						PasswordHash = authService.HashPassword("password5"),
						CreatedByEmployeeId = 2,
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Farmer
					{
						FarmerId = 4,
						FirstName = "Diana",
						LastName = "Black",
						Email = "diana.black@example.com",
						PasswordHash = authService.HashPassword("password6"),
						CreatedByEmployeeId = 2,
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Farmer
					{
						FarmerId = 5,
						FirstName = "Eve",
						LastName = "Gray",
						Email = "eve.gray@example.com",
						PasswordHash = authService.HashPassword("password7"),
						CreatedByEmployeeId = 1,
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					}
				);
			}

			// Seed Categories
			if (!context.Categories.Any())
			{
				context.Categories.AddRange(
					new Category { CategoryId = 1, Name = "Fruit", CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Category { CategoryId = 2, Name = "Vegetable", CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Category { CategoryId = 3, Name = "Livestock", CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Category { CategoryId = 4, Name = "Green Technology", CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Category { CategoryId = 5, Name = "Equipment", CreatedOn = DateTime.UtcNow, IsDeleted = false }
				);
			}

			// Seed Products
			if (!context.Products.Any())
			{
				context.Products.AddRange(
					// Fruit & Veg
					new Product { ProductId = 1, Name = "Apple", Category = "Fruit", Price = 1.5, FarmerId = 1, Image = LoadImage("apple"), CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Product { ProductId = 2, Name = "Tomato", Category = "Vegetable", Price = 1.2, FarmerId = 2, Image = LoadImage("tomato"), CreatedOn = DateTime.UtcNow, IsDeleted = false },

					// Livestock
					new Product { ProductId = 3, Name = "Angus Cattle", Category = "Livestock", Price = 1200, FarmerId = 3, Image = LoadImage("angus_cattle"), CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Product { ProductId = 4, Name = "Free-range Chickens (per dozen)", Category = "Livestock", Price = 60, FarmerId = 4, Image = LoadImage("chickens"), CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Product { ProductId = 5, Name = "Boer Goats", Category = "Livestock", Price = 250, FarmerId = 5, Image = LoadImage("goat"), CreatedOn = DateTime.UtcNow, IsDeleted = false },

					// Green Technology
					new Product { ProductId = 6, Name = "Solar Irrigation Pump", Category = "Green Technology", Price = 3200, FarmerId = 1, Image = LoadImage("solar_pump"), CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Product { ProductId = 7, Name = "Wind Turbine Kit (Small Farm)", Category = "Green Technology", Price = 5500, FarmerId = 2, Image = LoadImage("wind_turbine"), CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Product { ProductId = 8, Name = "Biogas Digester Unit", Category = "Green Technology", Price = 4100, FarmerId = 3, Image = LoadImage("biogas_unit"), CreatedOn = DateTime.UtcNow, IsDeleted = false },

					// Equipment
					new Product { ProductId = 9, Name = "Compost Spreader", Category = "Equipment", Price = 750, FarmerId = 4, Image = LoadImage("compost_spreader"), CreatedOn = DateTime.UtcNow, IsDeleted = false },
					new Product { ProductId = 10, Name = "Tractor (Used)", Category = "Equipment", Price = 8500, FarmerId = 5, Image = LoadImage("tractor"), CreatedOn = DateTime.UtcNow, IsDeleted = false }
				);
			}

			// Save changes to the database
			context.SaveChanges();
		}
	}
}