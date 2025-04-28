using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.Services;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;


[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ISupplierService _supplierService;
    public DashboardController(IProductService productService, ICategoryService categoryService, ISupplierService supplierService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _supplierService = supplierService;

    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllForDashboardAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new ProductViewModel
        {
            Categories = await _categoryService.GetAllAsync(),
            Suppliers = await _supplierService.GetAllAsync()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await _categoryService.GetAllAsync();
            vm.Suppliers = await _supplierService.GetAllAsync();
            return View(vm);
        }

        var product = new Product
        {
            Name = vm.Name,
            CategoryId = vm.CategoryId,
            SupplierId = vm.SupplierId,
            SalePrice = vm.SalePrice,
            Quantity = 0 // Quantity will be increased through Purchase Orders
        };

        await _productService.AddAsync(product);
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        var vm = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            SalePrice = product.SalePrice,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            Categories = await _categoryService.GetAllAsync(),
            Suppliers = await _supplierService.GetAllAsync()
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await _categoryService.GetAllAsync();
            vm.Suppliers = await _supplierService.GetAllAsync();
            return View(vm);
        }

        var product = new Product
        {
            Id = vm.Id,
            Name = vm.Name,
            SalePrice = vm.SalePrice,
            CategoryId = vm.CategoryId,
            SupplierId = vm.SupplierId,
            Quantity = 0 // Quantity won't be changed here; handled by purchase orders
        };

        await _productService.UpdateAsync(product);
        return RedirectToAction("Index", "Dashboard");
    }


}
