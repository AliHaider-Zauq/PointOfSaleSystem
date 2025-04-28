using Microsoft.EntityFrameworkCore;
using PointOfSaleSystem.Database;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.ViewModels;
using PointOfSaleSystem.Services.Interfaces;

namespace PointOfSaleSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductListViewModel>> GetAllForDashboardAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            var latestPurchasePrices = await _context.PurchaseOrderItems
                .Include(p => p.PurchaseOrder)
                .GroupBy(p => p.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    LatestPrice = g.OrderByDescending(p => p.PurchaseOrder.OrderDate)
                                   .Select(p => p.PurchasePrice)
                                   .FirstOrDefault()
                }).ToDictionaryAsync(x => x.ProductId, x => x.LatestPrice);

            return products.Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                SalePrice = p.SalePrice,
                Quantity = _context.PurchaseOrderItems
                            .Where(b => b.ProductId == p.Id)
                            .Sum(b => b.RemainingQuantity),
                LatestPurchasePrice = latestPurchasePrices.ContainsKey(p.Id) ? latestPurchasePrices[p.Id] : null,
                CategoryName = p.Category?.Name ?? "N/A",
                SupplierName = p.Supplier?.Name ?? "N/A"
            }).ToList();
        }

        // ✅ For Public Product Page (no purchase price)
        public async Task<IEnumerable<ProductListViewModel>> GetAllForProductIndexAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            return products.Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                SalePrice = p.SalePrice,
                Quantity = _context.PurchaseOrderItems
                            .Where(b => b.ProductId == p.Id)
                            .Sum(b => b.RemainingQuantity),
                CategoryName = p.Category?.Name ?? "N/A",
                SupplierName = p.Supplier?.Name ?? "N/A"
            }).ToList();
        }

        // ✅ Get single product by ID
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // ✅ Add new product
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // ✅ Update product
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // ✅ Delete product
        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
