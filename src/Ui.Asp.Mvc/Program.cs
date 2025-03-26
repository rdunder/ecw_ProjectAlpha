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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.WriteIndented = true;
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));


builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<IUserService, UserService>();


//  Asp webapp specific services
builder.Services.AddTransient<InitService>();
builder.Services.AddScoped<ImageManager>();




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
    opt.AccessDeniedPath = "/Auth/Login";
    opt.Cookie.Name = "AlphaAuthCookie";
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    opt.LoginPath = "/Auth/Login";
    opt.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    opt.SlidingExpiration = true;
});

builder.Services.Configure<CookiePolicyOptions>(opt =>
{
    opt.CheckConsentNeeded = context => true;
    opt.MinimumSameSitePolicy = SameSiteMode.Strict;
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


app.Run();
