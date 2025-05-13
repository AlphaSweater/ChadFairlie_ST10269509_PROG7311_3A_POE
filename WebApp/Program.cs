using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Models;
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

			// Configure middleware
			ConfigureMiddleware(app);

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
			builder.Services.AddScoped<UserSessionService>();
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