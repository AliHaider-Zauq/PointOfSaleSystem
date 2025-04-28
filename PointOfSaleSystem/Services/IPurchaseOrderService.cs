using PointOfSaleSystem.Models;
using PointOfSaleSystem.ViewModels;

public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
    Task<bool> CreatePurchaseOrderAsync(PurchaseOrderViewModel model);
    Task<Product?> GetProductByIdAsync(int id);
    Task UpdateProductAsync(Product product);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
}
