using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Configure services
			ConfigureServices(builder);

			var app = builder.Build();
			CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

			// Configure middleware
			ConfigureMiddleware(app);

			// Seed the database
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<AgriDbContext>();
				var authService = services.GetRequiredService<AuthService>();
				DatabaseSeeder.Seed(context, authService);
			}

			app.Run();
		}

		private static void ConfigureServices(WebApplicationBuilder builder)
		{
			// Add controllers with views
			builder.Services.AddControllersWithViews();

			// Register AgriDbContext with SQLite
			builder.Services.AddDbContext<AgriDbContext>(options =>
				options.UseSqlite("Data Source=localAgriEnergy.db"));

			// Add session services
			builder.Services.AddDistributedMemoryCache(); // For storing session data in memory
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
				options.Cookie.HttpOnly = true; // Prevent JS access
				options.Cookie.IsEssential = true; // Required for GDPR compliance
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Always use secure cookies
			});

			// Add HttpContextAccessor
			builder.Services.AddHttpContextAccessor();

			// Add custom services
			builder.Services.AddScoped<AuthService>();

			// Add repositories and services
			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<ProductService>();

			builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
			builder.Services.AddScoped<FarmerService>();

			builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddScoped<EmployeeService>();

			// Add logging
			builder.Services.AddLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddConsole();
				logging.AddDebug();
			});
		}

		private static void ConfigureMiddleware(WebApplication app)
		{
			// Configure the HTTP request pipeline
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			// Use session before endpoints
			app.UseSession();

			app.UseAuthorization();

			// Map default controller route
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Auth}/{action=Auth}/{id?}");
		}
	}
}