using PointOfSaleSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointOfSaleSystem.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }

}
