using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            return await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems).ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<bool> CreatePurchaseOrderAsync(PurchaseOrderViewModel model)
        {
            var purchaseOrder = new PurchaseOrder
            {
                SupplierId = model.SupplierId,
                OrderDate = DateTime.UtcNow,
                PurchaseItems = new List<PurchaseOrderItem>()
            };

            foreach (var item in model.PurchaseItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null) continue;

                product.Quantity += item.Quantity;
                if (item.SalePrice.HasValue)
                    product.SalePrice = item.SalePrice.Value;

                _context.Products.Update(product);

                purchaseOrder.PurchaseItems.Add(new PurchaseOrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    RemainingQuantity = item.Quantity,
                    PurchasePrice = item.PurchasePrice
                });
            }

            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> GetProductByIdAsync(int id) => await _context.Products.FindAsync(id);
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _context.Products.ToListAsync();
        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync() => await _context.Suppliers.ToListAsync();
    }

}
