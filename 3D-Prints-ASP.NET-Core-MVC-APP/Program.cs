using _3D_Prints_APP.Data.Repositories;
using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using _3DPrintsAPP.Data.Models;

namespace _3DPrintsAPP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<IPrinterRepository, PrinterRepository>();
            builder.Services.AddScoped<IPrinterService, PrinterService>();

            builder.Services.AddScoped<IFilamentRepository, FilamentRepository>();
            builder.Services.AddScoped<IFilamentService, FilamentService>();

            builder.Services.AddScoped<IPrintRepository, PrintRepository>();
            builder.Services.AddScoped<IPrintService, PrintService>();

            builder.Services.AddScoped<IPrinterOptionRepository, PrinterOptionRepository>();
            builder.Services.AddScoped<IPrinterOptionService, PrinterOptionService>();

            builder.Services.AddScoped<IFilamentOptionRepository, FilamentOptionRepository>();
            builder.Services.AddScoped<IFilamentOptionService, FilamentOptionService>();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                ConfigureIdentityOptions(builder.Configuration, options);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
           
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();

            // checks after every request if user exists in Database
            app.Use(async (context, next) =>
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                    var signInManager = context.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

                    var user = await userManager.GetUserAsync(context.User);

                    if (user == null)
                    {
                        await signInManager.SignOutAsync();
                    }
                }

                await next();
            });

            app.UseAuthorization();

            app.MapStaticAssets();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }

        private static void ConfigureIdentityOptions(ConfigurationManager configuration, IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount =
                configuration.GetValue<bool>("Identity:RequireConfirmedAccount");

            options.Password.RequireDigit = configuration.GetValue<bool>("Identity:RequireDigit");

            options.Password.RequiredLength = configuration.GetValue<int>("Identity:RequireLength");

            options.Password.RequireUppercase = configuration.GetValue<bool>("Identity:RequireUppercase");

            options.Password.RequireNonAlphanumeric =
                configuration.GetValue<bool>("Identity:RequireNonAlphanumeric");

            options.Password.RequireLowercase = configuration.GetValue<bool>("Identity:RequireLowercase");
        }
    }
}
