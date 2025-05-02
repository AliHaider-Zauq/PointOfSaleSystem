using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardIndexViewModel> GetDashboardReportAsync()
        {
            var today = DateTime.UtcNow.Date;

            var todayOrders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderDate >= today && o.OrderDate < today.AddDays(1))
                .ToListAsync();

            var totalSalesToday = todayOrders
                .SelectMany(o => o.OrderItems)
                .Sum(i => i.ProductSalePrice * i.Quantity);

            var allOrderItems = await _context.OrderItems.ToListAsync();
            decimal totalRevenue = allOrderItems.Sum(i => i.ProductSalePrice * i.Quantity);
            decimal totalCost = allOrderItems.Sum(i => i.ProductPurchasePrice * i.Quantity);
            decimal totalProfit = totalRevenue - totalCost;

            var topCustomers = await _context.Orders
                .Where(o => o.Customer != null)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .GroupBy(o => o.Customer.Name)
                .Select(g => new TopCustomerViewModel
                {
                    CustomerName = g.Key,
                    OrdersCount = g.Count(),
                    TotalSpent = g.SelectMany(o => o.OrderItems)
                                  .Sum(i => i.ProductSalePrice * i.Quantity)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToListAsync();

            var topProducts = allOrderItems
     .Where(i => !string.IsNullOrEmpty(i.ProductName))
     .GroupBy(i => i.ProductName)
     .Select(g => new TopProductViewModel
     {
         ProductName = g.Key,
         QuantitySold = g.Sum(i => i.Quantity)
     })
     .Where(p => p.QuantitySold > 0) // ✅ Exclude products with zero quantity sold
     .OrderByDescending(p => p.QuantitySold)
     .Take(5)
     .ToList();


            var lowStock = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Quantity < 10)
                .Select(p => new ProductListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    SalePrice = p.SalePrice,
                    LatestPurchasePrice = null,
                    CategoryName = p.Category.Name,
                    SupplierName = ""
                }).ToListAsync();

            return new DashboardIndexViewModel
            {
                TotalSalesToday = totalSalesToday,
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit,
                TopCustomers = topCustomers,
                TopProducts = topProducts,
                LowStockProducts = lowStock
            };
        }
    }
}
