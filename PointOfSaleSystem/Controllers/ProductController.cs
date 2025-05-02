using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Services;
using PointOfSaleSystem.Services.Interfaces;

[Authorize]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // 🔵 Public-facing or Cashier view (without Purchase Price)
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllForProductIndexAsync();
        return View(products);
    }
}
