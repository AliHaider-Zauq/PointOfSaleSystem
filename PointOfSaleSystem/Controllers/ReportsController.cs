using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.ViewModels;


public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;

        // 🔹 1. Today's Sales
        var todayOrders = await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.OrderDate >= today && o.OrderDate < today.AddDays(1))
            .ToListAsync();

        var totalSalesToday = todayOrders
            .SelectMany(o => o.OrderItems)
            .Sum(i => i.ProductSalePrice * i.Quantity);

        // 🔹 2. Total Revenue & Profit
          var allOrderItems = await _context.OrderItems.ToListAsync();
        
          decimal totalRevenue = allOrderItems.Sum(i => i.ProductSalePrice * i.Quantity);
          decimal totalCost = allOrderItems.Sum(i => i.ProductPurchasePrice * i.Quantity);
        decimal totalProfit = totalRevenue - totalCost;


        // 🔹 3. Top Customers
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

        ViewBag.TopCustomers = topCustomers;


        //Top Selling Products
        var topProducts = allOrderItems
     .Where(i => !string.IsNullOrEmpty(i.ProductName))
     .GroupBy(i => i.ProductName)
     .Select(g => new TopProductViewModel
     {
         ProductName = g.Key,
         QuantitySold = g.Sum(i => i.Quantity)
     })
     .OrderByDescending(p => p.QuantitySold)
     .Take(5)
     .ToList();

        ViewBag.TopProducts = topProducts;

        // 🔹 5. Low Stock Products
        var lowStock = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Quantity < 10)
            .ToListAsync();

        // 🔄 Pass data to ViewBag
        ViewBag.TotalSalesToday = totalSalesToday;
        ViewBag.TotalRevenue = totalRevenue;
        ViewBag.TotalProfit = totalProfit;
        ViewBag.TopCustomers = topCustomers;
        ViewBag.TopProducts = topProducts;
        ViewBag.LowStockProducts = lowStock;

        return View();
    }
}
