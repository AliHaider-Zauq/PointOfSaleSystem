using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSaleSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<bool> PlaceOrderAsync(OrderViewModel model)
        {
            var order = new Order
            {
                CustomerId = model.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in model.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || item.Quantity > product.Quantity)
                    return false;

                var purchaseBatches = await _context.PurchaseOrderItems          
                    .Where(p => p.ProductId == item.ProductId && p.RemainingQuantity > 0)
                    .OrderBy(p => p.PurchaseOrder.OrderDate)
                    .ToListAsync();

                int quantityToSell = item.Quantity;

                foreach (var batch in purchaseBatches)
                {
                    if (quantityToSell == 0) break;

                    int qtyFromBatch = Math.Min(batch.RemainingQuantity, quantityToSell);

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = qtyFromBatch,
                        ProductPurchasePrice = batch.PurchasePrice,
                        ProductSalePrice = product.SalePrice
                    });

                    batch.RemainingQuantity -= qtyFromBatch;
                    quantityToSell -= qtyFromBatch;
                }

                product.Quantity -= item.Quantity;
                _context.Products.Update(product);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _context.Products.ToListAsync();
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync() => await _context.Customers.ToListAsync();

        public async Task<Order> GetOrderDetailsByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems) 
                .ThenInclude(oi => oi.Product) 
                .FirstOrDefaultAsync(o => o.Id == orderId); 
        }
    }

}
