using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;

public class PurchaseOrderController : Controller
{
    private readonly IPurchaseOrderService _purchaseService;

    public PurchaseOrderController(IPurchaseOrderService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _purchaseService.GetAllPurchaseOrdersAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new PurchaseOrderViewModel
        {
            Products = await _purchaseService.GetAllProductsAsync(),
            Suppliers = await _purchaseService.GetAllSuppliersAsync()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PurchaseOrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Products = await _purchaseService.GetAllProductsAsync();
            model.Suppliers = await _purchaseService.GetAllSuppliersAsync();
            return View(model);
        }

        var result = await _purchaseService.CreatePurchaseOrderAsync(model);
        if (result)
            return RedirectToAction("Index");

        ModelState.AddModelError("", "Something went wrong while creating the purchase order.");
        model.Products = await _purchaseService.GetAllProductsAsync();
        model.Suppliers = await _purchaseService.GetAllSuppliersAsync();
        return View(model);
    }
}
