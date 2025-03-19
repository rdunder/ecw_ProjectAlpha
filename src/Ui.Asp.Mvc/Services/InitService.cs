using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Dtos;
using Service.Interfaces;
using Service.Models;

namespace Ui.Asp.Mvc.Services;

public class InitService(
     ICustomerService customerService,
     IProjectService projectService,
     IRoleService roleService,
     IStatusService statusService,
     IUserAddressService userAddressService,
     IUserService userService)
{

    private readonly ICustomerService _customerService = customerService;
    private readonly IProjectService _projectService = projectService;
    private readonly IRoleService _roleService = roleService;
    private readonly IStatusService _statusService = statusService;
    private readonly IUserAddressService _userAddressService = userAddressService;
    private readonly IUserService _userService = userService;

    public async Task InitCreate()
    {
        await CreateStatuses();
        await CreateRoles();
        await CreateCustomers();

        await CreateUsers();
        await AddAdminRole();

        await CreateProjects();
    }

    private async Task CreateStatuses()
    {
        await _statusService.CreateAsync(new StatusDto() { StatusName = "Pending" });
        await _statusService.CreateAsync(new StatusDto() { StatusName = "Active" });
        await _statusService.CreateAsync(new StatusDto() { StatusName = "Closed" });
    }
    private async Task CreateRoles()
    {
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Trainee" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Fullstack Developer" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Frontend Developer" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Backend Developer" });
        await _roleService.CreateAsync(new RoleDto() { RoleName = "Administrator" });
    }
    private async Task CreateCustomers()
    {
        await _customerService.CreateAsync(new CustomerDto()
        {
            CustomerName = "Test Customer 01",
            Email = "test.customer.1@domain.com"
        });

        await _customerService.CreateAsync(new CustomerDto()
        {
            CustomerName = "Test Customer 02",
            Email = "test.customer.2@domain.com"
        });

        await _customerService.CreateAsync(new CustomerDto()
        {
            CustomerName = "Test Customer 03",
            Email = "test.customer.3@domain.com"
        });
    }
    private async Task CreateUsers()
    {
        var admin = new UserDto()
        {
            FirstName = "Super",
            LastName = "User",
            Email = "admin@domain.com",
            PhoneNumber = "+46 743 897 356",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var user = new UserDto()
        {
            FirstName = "Olle",
            LastName = "Dörpen",
            Email = "olle@domain.com",
            PhoneNumber = "+46 765 829 356",
            Password = "Password123!",
            ConfirmPassword = "Password123!"      
        };

        var adminResult = await _userService.CreateAsync(admin);
        var userResult = await _userService.CreateAsync(user);
    }
    private async Task AddAdminRole()
    {
        var admin = new UserModel()
        {
            FirstName = "Super",
            LastName = "User",
            Email = "admin@domain.com",
            PhoneNumber = "+46 743 897 356",
        };

        var result = await _userService.AddToRoleAsync(admin, "Administrator");
    }
    private async Task CreateProjects()
    {
        var customers = await _customerService.GetAllAsync();
        var customerId = customers.FirstOrDefault().Id;

        var statuses = await _statusService.GetAllAsync();
        var statusId = statuses.FirstOrDefault().Id;

        var project = new ProjectDto()
        {
            Name = "Test Project",
            Description = "Description for Test Project",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            Budget = 850,

            StatusId = statusId,
            CustomerId = customerId
        };

        await _projectService.CreateAsync(project);

    }
}
