using PointOfSaleSystem.Models;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListViewModel>> GetAllForDashboardAsync();
        Task<IEnumerable<ProductListViewModel>> GetAllForProductIndexAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

    }

}
