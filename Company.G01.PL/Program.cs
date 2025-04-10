using Company.G01.BLL;
using Company.G01.BLL.Interfaces;
using Company.G01.BLL.Repositories;
using Company.G01.DAL.Data.Contexts;
using Company.G01.DAL.Models;
using Company.G01.PL.Helpers;
using Company.G01.PL.Mapping;
using Company.G01.PL.Settings;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.G01.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

            }).AddGoogle(o =>
            {
                o.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;

            }).AddFacebook(o =>
            {
                o.ClientId = builder.Configuration["Authentication:Facebook:AppId"];
                o.ClientSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register built-in MVC service
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow DI for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Allow DI For EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI for CompanyDbContext

            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));

            


            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>();


            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath = "/Account/AccessDenied";
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

            app.Run();
        }
    }
}
