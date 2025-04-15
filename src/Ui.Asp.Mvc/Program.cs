using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Service.Interfaces;
using Service.Services;
using Data.Interfaces;
using Data.Repositories;
using Ui.Asp.Mvc.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Ui.Asp.Mvc.Hubs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("LocalDb") ?? throw new ArgumentNullException($"Failed to get connectionstring:\n{nameof(args)}");

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.WriteIndented = true;
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(connectionString));


builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IJobTitleRepository, JobTitleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationDismissedRepository, NotificationDismissedRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IUserService, UserService>();


//  Asp webapp specific services
builder.Services.AddTransient<InitService>();
builder.Services.AddScoped<ImageManager>();
builder.Services.AddScoped<MailService>();




builder.Services.AddIdentity<UserEntity, RoleEntity>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.User.RequireUniqueEmail = true;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Auth/Login";
    opt.AccessDeniedPath = "/Auth/AdminLogin";

    opt.Cookie.Name = "AlphaAuthCookie";
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SameSite = SameSiteMode.None;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    opt.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    opt.SlidingExpiration = true;
});

//  Decided not to use policies for authorization
//builder.Services.AddAuthorization(opt =>
//{
//    opt.AddPolicy("Admins", policy => policy.RequireRole("Administrator"));
//    opt.AddPolicy("Managers", policy => policy.RequireRole("Administrator", "Manager"));
//    opt.AddPolicy("Authorized", policy => policy.RequireRole(
//        "Administrator", 
//        "Fullstack Developer", 
//        "Frontend Developer",
//        "Backend Developer"));
//});

builder.Services.Configure<CookiePolicyOptions>(opt =>
{
    opt.CheckConsentNeeded = context => true;
    opt.MinimumSameSitePolicy = SameSiteMode.None;
});


builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(opt =>
    {
        opt.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        opt.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        opt.Scope.Add("profile");
        opt.ClaimActions.MapJsonKey("picture", "picture", "url");
    })
    .AddGitHub(opt =>
    {
        opt.ClientId = builder.Configuration["Authentication:Github:ClientId"]!;
        opt.ClientSecret = builder.Configuration["Authentication:Github:ClientSecret"]!;
        opt.Scope.Add("user:email");
        opt.Scope.Add("read:user");

        opt.ClaimActions.MapJsonKey("picture", "avatar_url");

        opt.Events.OnCreatingTicket = ctx =>
        {
            if (ctx.User.TryGetProperty("name", out var name) && !string.IsNullOrEmpty(name.GetString()) && ctx.Identity != null)
            {
                var fullName = name.GetString().Split(' ', 2);

                ctx.Identity.AddClaim(new Claim(ClaimTypes.GivenName, fullName.Length > 0 ? fullName[0] : ""));
                ctx.Identity.AddClaim(new Claim(ClaimTypes.Surname, fullName.Length > 1 ? fullName[1] : ""));
            }

            return Task.CompletedTask;
        };
    });

var app = builder.Build();


app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
