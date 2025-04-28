using PointOfSaleSystem.Models;
using PointOfSaleSystem.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointOfSaleSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<bool> PlaceOrderAsync(OrderViewModel model);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Order> GetOrderDetailsByIdAsync(int orderId);
    }


}
