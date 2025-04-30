using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Services
{
    public class ReturnOrderService : IReturnOrderService
    {
        private readonly ApplicationDbContext _context;

        public ReturnOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Load Order Info to populate return form

       public async Task<ReturnOrderViewModel?> GetReturnFormByOrderIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return null;

            var viewModel = new ReturnOrderViewModel
            {
                OrderId = order.Id,
                CustomerName = order.Customer?.Name ?? "Walk-in",
                ReturnDate = DateTime.UtcNow,
                ReturnItems = order.OrderItems.Select(oi => new ReturnItemViewModel
                {
                    OrderItemId = oi.Id,
                    ProductName = oi.Product.Name,
                    MaxQuantity = oi.Quantity,
                    Quantity = 0, // default user input
                    PurchasePrice = oi.ProductPurchasePrice,
                    SalePrice = oi.ProductSalePrice
                }).ToList()
            };

            return viewModel;
        }

        // Process return and update FIFO purchase batches
        public async Task<bool> ProcessReturnAsync(ReturnOrderViewModel model)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order == null) return false;

            var returnOrder = new ReturnOrder
            {
                OrderId = order.Id,
                ReturnDate = DateTime.UtcNow,
                ReturnItems = new List<ReturnOrderItem>()
            };

            foreach (var item in model.ReturnItems)
            {
                if (item.Quantity <= 0) continue;

                var orderItem = await _context.OrderItems
                    .Include(oi => oi.Product)
                    .FirstOrDefaultAsync(oi => oi.Id == item.OrderItemId);

                if (orderItem == null || item.Quantity > orderItem.Quantity)
                    continue;

                // Save return item
                returnOrder.ReturnItems.Add(new ReturnOrderItem
                {
                    OrderItemId = orderItem.Id,
                    Quantity = item.Quantity,
                    PurchasePrice = orderItem.ProductPurchasePrice,
                    SalePrice = orderItem.ProductSalePrice
                });

                var product = orderItem.Product;
                product.Quantity += item.Quantity; // 🔥 This line fixes your issue
                _context.Entry(product).State = EntityState.Modified;

                // Update FIFO purchase batches
                var remainingQty = item.Quantity;

                var batches = await _context.PurchaseOrderItems
                    .Where(poi => poi.ProductId == product.Id)
                    .OrderBy(poi => poi.Id)
                    .ToListAsync();

                foreach (var batch in batches)
                {
                    if (batch.RemainingQuantity >= batch.Quantity) continue;

                    var soldFromThisBatch = batch.Quantity - batch.RemainingQuantity;
                    var toReturn = Math.Min(remainingQty, soldFromThisBatch);
                    if (toReturn <= 0) continue;

                    batch.RemainingQuantity += toReturn;
                    _context.Entry(batch).State = EntityState.Modified;

                    remainingQty -= toReturn;
                    if (remainingQty <= 0) break;
                }

                // Reduce quantity from OrderItem if tracking net sold
                orderItem.Quantity -= item.Quantity;
                _context.Entry(orderItem).State = EntityState.Modified;
            }

            _context.ReturnOrders.Add(returnOrder);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
