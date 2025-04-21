using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;
public class CustomerRepository(AppDbContext context) :
    BaseRepository<CustomerEntity>(context), ICustomerRepository
{
}
