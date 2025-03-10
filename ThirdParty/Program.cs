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
using System.Security.Claims;
using Infrastructure.FileUploadService;
using Infrastructure.FileUploadService.FileUploadOnService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Events;
namespace ThirdParty
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



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

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddAuthentication(op =>
            {
                op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(op =>
            {
                op.LoginPath = "/Account/Login";
                op.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, op =>
            {
                op.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
                op.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
            });

            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();
            builder.Services.AddScoped(typeof(IBaseRepositrory<>), typeof(BaseRepositrory<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IFileUploadUloudinary, FileUploadUloudinary>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            builder.Services.AddScoped<ISendSms, SendSms>();
            builder.Services.AddScoped<ISendEmail, SendEmails>();

            builder.Services.AddScoped<IFileUploadService>(provider =>
            {
                var webHostEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
                return new FileUploadService(webHostEnvironment.WebRootPath);
            });

            // configuration
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<SMSSetting>(builder.Configuration.GetSection("SMSSetting"));
            builder.Services.Configure<Attachments>(builder.Configuration.GetSection("Attachments"));
            builder.Services.Configure<AdminData>(builder.Configuration.GetSection("AdminSettings"));

            builder.Services.AddAutoMapper(typeof(MappingApplication));

            builder.Services.AddMemoryCache(op =>
            {
                op.SizeLimit = 1024 * 1024 * 100;
            });
            //builder.Host.UseSerilog((context, services, config) =>
            //{
            //    config
            //   .MinimumLevel.Information()
            //        .WriteTo.Console()
            //        .WriteTo.File(
            //            path: "logs/log.json",
            //            rollingInterval: RollingInterval.Day,
            //            formatter: new JsonFormatter()) 
            //        .Enrich.FromLogContext();
            //});

            //Add support to logging with SERILOG


            builder.Services.AddControllersWithViews();


            var app = builder.Build();
            //app.UseSerilogRequestLogging(options =>
            //{
            //    options.GetLevel = (httpContext, elapsed, ex) =>
            //        ex != null || httpContext.Response.StatusCode >= 400
            //            ? LogEventLevel.Warning 
            //            : LogEventLevel.Information; 
            //});


            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<CommissionScheme>>();
                var unitOfWorkshipping = scope.ServiceProvider.GetRequiredService<IUnitOfWork<PricesToshipping>>();
                var unitOfWorksaccount = scope.ServiceProvider.GetRequiredService<IUnitOfWork<Account>>();
                var unitOfWorkuser = scope.ServiceProvider.GetRequiredService<IUnitOfWork<ApplicationUser>>();
                var aurhrep = scope.ServiceProvider.GetRequiredService<IAuthRepository>();
                var options = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<AdminData>>();
                await SeederData.SeedDataInCommissionScheme(unitOfWork);
                await SeederData.SeedDataShiping(unitOfWorkshipping);
                await SeederData.SeedRolesAsync(roleManager);
                await SeederData.SeedDataAccounts(unitOfWorksaccount);
                await SeederData.SeedAdminInfo(unitOfWorkuser, aurhrep, options);
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
