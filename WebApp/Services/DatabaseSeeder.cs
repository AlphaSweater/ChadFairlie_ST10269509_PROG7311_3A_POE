using WebApp.Models;

namespace WebApp.Services
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

		public static void Seed(AgriDbContext context)
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
						FirstName = "John",
						LastName = "Doe",
						Email = "john.doe@example.com",
						PasswordHash = "hashedpassword1",
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Employee
					{
						EmployeeId = 2,
						FirstName = "Jane",
						LastName = "Smith",
						Email = "jane.smith@example.com",
						PasswordHash = "hashedpassword2",
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
						PasswordHash = "hashedpassword3",
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
						PasswordHash = "hashedpassword4",
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
						PasswordHash = "hashedpassword5",
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
						PasswordHash = "hashedpassword6",
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
						PasswordHash = "hashedpassword7",
						CreatedByEmployeeId = 1,
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					}
				);
			}

			// Seed Products
			if (!context.Products.Any())
			{
				context.Products.AddRange(
					new Product
					{
						ProductId = 1,
						Name = "Apple",
						Category = "Fruit",
						Price = 1.5,
						FarmerId = 1,
						Image = LoadImage("apple"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 2,
						Name = "Banana",
						Category = "Fruit",
						Price = 0.8,
						FarmerId = 2,
						Image = LoadImage("banana"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 3,
						Name = "Carrot",
						Category = "Vegetable",
						Price = 0.5,
						FarmerId = 3,
						Image = LoadImage("carrot"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 4,
						Name = "Tomato",
						Category = "Vegetable",
						Price = 1.2,
						FarmerId = 4,
						Image = LoadImage("tomato"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 5,
						Name = "Potato",
						Category = "Vegetable",
						Price = 0.6,
						FarmerId = 5,
						Image = LoadImage("potato"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 6,
						Name = "Strawberry",
						Category = "Fruit",
						Price = 2.5,
						FarmerId = 1,
						Image = LoadImage("strawberry"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 7,
						Name = "Cucumber",
						Category = "Vegetable",
						Price = 1.0,
						FarmerId = 2,
						Image = LoadImage("cucumber"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 8,
						Name = "Orange",
						Category = "Fruit",
						Price = 1.3,
						FarmerId = 3,
						Image = LoadImage("orange"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 9,
						Name = "Lettuce",
						Category = "Vegetable",
						Price = 0.9,
						FarmerId = 4,
						Image = LoadImage("lettuce"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					},
					new Product
					{
						ProductId = 10,
						Name = "Blueberry",
						Category = "Fruit",
						Price = 3.0,
						FarmerId = 5,
						Image = LoadImage("blueberry"),
						CreatedOn = DateTime.UtcNow,
						IsDeleted = false
					}
				);
			}

			// Save changes to the database
			context.SaveChanges();
		}
	}
}