using Microsoft.EntityFrameworkCore;
using SignalRProject.DbContextFolder;
using SignalRProject.HubFolder;

namespace SignalRProject
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<ChatDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("ChatConnectionString"));
			});
			builder.Services.AddSignalR();
			
			// Configure Who Can Access MY Hub
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("default", p => 
				{
					p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
				});
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

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			// Middleware To Route "ChatHub"
			app.MapHub<ChatHub>("/Chathub");
            app.UseCors("default");

            app.Run();
		}
	}
}
