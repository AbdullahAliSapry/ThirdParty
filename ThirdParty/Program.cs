using ThirdParty.Utilities;
using Bll.Mapping;
using DAl;
using DAl.IRepository;
using DAl.Models;
using DAl.Repository;
using Infrastructure.ApiotService.Interfaces;
using Infrastructure.ApiotService.Services;
using Infrastructure.SendingEmails;
using Infrastructure.SendingSms;
using Infrastructure.Translationservice;
using Infrastructure.Utlities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Twilio.Base;
namespace ThirdParty
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);




            builder.Services.AddControllersWithViews();


            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar"),
            };

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;
            });




            builder.Services.AddScoped(typeof(IApiService<>), typeof(ApiService<>));
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddSingleton<ITransaltionService, TransaltionService>();
            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DevlopementConnection")));

            builder.Services.AddIdentity<ApplicationUser,
                IdentityRole>(op =>
                {
                    op.SignIn.RequireConfirmedAccount = false;


                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

                    builder.Services.ConfigureApplicationCookie(op =>
                    {
                        op.LoginPath = "/Account/Login"; 
                       
                    
                    });

            builder.Services.AddAuthentication(op =>
            {
                op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(op =>
            {
                //op.Cookie.Name = ".AspNetCore.Correlation.Google";
                op.LoginPath = "/Account/Login";
                op.AccessDeniedPath = "/Account/AccessDenied";
                op.LogoutPath = "/Account/Logout";
                op.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                op.SlidingExpiration = true;
            })
                   .AddGoogle(GoogleDefaults.AuthenticationScheme, op =>
                   {
                       op.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
                       op.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
                   });




            builder.Services.AddScoped(typeof(IBaseRepositrory<>), typeof(BaseRepositrory<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            builder.Services.AddScoped<ISendSms, SendSms>();
            builder.Services.AddScoped<ISendEmail, SendEmails>();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<SMSSetting>(builder.Configuration.GetSection("SMSSetting"));

            builder.Services.AddAutoMapper(typeof(MappingApplication));

            builder.Services.AddMemoryCache(op =>
            {
                op.SizeLimit = 1024 * 1024 * 100;
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleSeeder.SeedRolesAsync(roleManager);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.Services
                .CreateScope().ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
