using Service.Models;

namespace Ui.Asp.Mvc.Models;

public class CustomersViewModel
{
    public IEnumerable<CustomerModel> customers { get; set; }
    public CustomerFormViewModel Form { get; set; } = new();
}
