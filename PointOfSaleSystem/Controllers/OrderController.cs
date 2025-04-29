using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new OrderViewModel
        {
            Products = await _orderService.GetAllProductsAsync(),
            Customers = await _orderService.GetAllCustomersAsync()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Products = await _orderService.GetAllProductsAsync();
            model.Customers = await _orderService.GetAllCustomersAsync();
            return View(model);
        }

        var result = await _orderService.PlaceOrderAsync(model);
        if (result)
            return RedirectToAction("Success");

        ModelState.AddModelError("", "Not enough stock or invalid entry.");
        model.Products = await _orderService.GetAllProductsAsync();
        model.Customers = await _orderService.GetAllCustomersAsync();
        return View(model);
    }

    public IActionResult Success()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetOrderDetailsByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

}
