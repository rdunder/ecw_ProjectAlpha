using Service.Dtos;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System.ComponentModel.DataAnnotations;

namespace Ui.Asp.Mvc.Models;

public class ProjectsViewModel
{  
    public IEnumerable<StatusModel> Statuses { get; set; } = new List<StatusModel>();

    public IEnumerable<CustomerModel> Customers { get; set; } = new List<CustomerModel>();

    public IEnumerable<ProjectModel> Projects { get; set; } = new List<ProjectModel>();

    public ProjectFormViewModel form { get; set; } = new();

    
}
