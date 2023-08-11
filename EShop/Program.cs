using EShop.Data;
using EShop.Repositories;
using EShop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<EShopDBContext>();

            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<CartRepository>();
            builder.Services.AddScoped<CartItemRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<BannerRepository>();
			builder.Services.AddScoped<FeedbackRepository>();

			builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<BannerService>();
			builder.Services.AddScoped<FeedbackService>();

			builder.Services.AddScoped<VnPayService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.Cookie.Name = "E-Shop";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/Customer/Login";
				//options.AccessDeniedPath = "/Customer/Login";
			});

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();

            app.Run();
        }
    }
}