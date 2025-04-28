using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSaleSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Customer>> GetAllAsync() =>
            await _context.Customers.ToListAsync();

        public async Task<Customer?> GetByIdAsync(int id) =>
            await _context.Customers.FindAsync(id);

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }

}
