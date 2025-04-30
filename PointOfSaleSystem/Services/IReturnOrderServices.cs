using PointOfSaleSystem.Models;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Services.Interfaces
{
    public interface IReturnOrderService
    {
        Task<ReturnOrderViewModel?> GetReturnFormByOrderIdAsync(int orderId);
        Task<bool> ProcessReturnAsync(ReturnOrderViewModel model);
    }

}
