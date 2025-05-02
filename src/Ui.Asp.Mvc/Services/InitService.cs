using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using System.Runtime.CompilerServices;

namespace Ui.Asp.Mvc.Services;

public class InitService(
     ICustomerService customerService,
     IProjectService projectService,
     IRoleService roleService,
     IStatusService statusService,
     IUserAddressService userAddressService,
     IJobTitleService jobTitleService,
     IUserService userService,
     IConfiguration config)
{

    private readonly ICustomerService _customerService = customerService;
    private readonly IProjectService _projectService = projectService;
    private readonly IRoleService _roleService = roleService;
    private readonly IStatusService _statusService = statusService;
    private readonly IUserAddressService _userAddressService = userAddressService;
    private readonly IJobTitleService _jobTitleService = jobTitleService;
    private readonly IUserService _userService = userService;
    private readonly IConfiguration _config = config;

    public async Task InitCreate()
    {
        await CreateStatuses();
        await CreateRoles();
        await CreateJobTitles();

        await CreateCustomers();
        await CreateAdmin();
    }

    private async Task CreateStatuses()
    {
        await _statusService.CreateAsync(new StatusDto() { StatusName = "Pending" });
        await _statusService.CreateAsync(new StatusDto() { StatusName = "Active" });
        await _statusService.CreateAsync(new StatusDto() { StatusName = "Closed" });
    }
    private async Task CreateRoles()
    {
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Viewer" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "User" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Manager" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Administrator" });
    }

    private async Task CreateJobTitles()
    {
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Guest" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Trainee" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Fullstack Developer" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Frontend Developer" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Backend Developer" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Designer" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Project Manager" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Team Lead" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Scrum Master" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Product Owner" });
        await _jobTitleService.CreateAsync(new JobTitleDto() { Title = "Sys Admin" });
    }

    private async Task CreateCustomers()
    {
        await _customerService.CreateAsync(new CustomerDto()
        {
            CustomerName = "Test Customer 01",
            Email = "test.customer.1@domain.com"
        });
    }
    private async Task CreateAdmin()
    {
        var adminEmail = _config["DefaultAdmin:Email"] ?? throw new NullReferenceException("Default Admin is not configured in appsettings.json");
        var adminPassword = _config["DefaultAdmin:Password"] ?? throw new NullReferenceException("Default Admin is not configured in appsettings.json");
        var titles = await _jobTitleService.GetAllAsync();
        Guid t = titles.FirstOrDefault(t => t.Title == "Sys Admin").Id;


        var admin = new UserDto()
        {
            FirstName = "Super",
            LastName = "User",
            Email = adminEmail,
            Password = adminPassword,
            JobTitleId = t,
        };

        var adminResult = await _userService.CreateAsync(admin);
        var adToRoleResult = await _userService.AddToRoleAsync("admin@domain.com", "Administrator");
    }
}
