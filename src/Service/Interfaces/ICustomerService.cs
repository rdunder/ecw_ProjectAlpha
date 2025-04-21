using Service.Dtos;
using Service.Models;

namespace Service.Interfaces;
public interface ICustomerService : IService<CustomerModel, CustomerDto>
{
}
