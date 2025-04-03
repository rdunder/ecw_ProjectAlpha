using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Models;

public class ProjectsViewModel
{  
    public IEnumerable<StatusModel> Statuses { get; set; } = [];

    public IEnumerable<CustomerModel> Customers { get; set; } = [];
        

    public IEnumerable<ProjectModel> Projects { get; set; } = [];
    public IEnumerable<UserModel> Users { get; set; } = [];

    public ProjectFormViewModel form { get; set; } = new();
}
