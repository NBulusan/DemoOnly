
using CustomerInfo.Dto;
using CustomerInfo.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerInfo.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(CustomerDto customer, CancellationToken ct);
    }
}