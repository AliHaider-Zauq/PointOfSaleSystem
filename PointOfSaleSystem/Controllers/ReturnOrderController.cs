using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;
using PointOfSaleSystem.Services;
using PointOfSaleSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class ReturnOrderController : Controller
{
    private readonly IReturnOrderService _returnOrderService;

    public ReturnOrderController(IReturnOrderService returnOrderService)
    {
        _returnOrderService = returnOrderService;
    }

    // GET: /ReturnOrder/Create/{orderId}
    public async Task<IActionResult> Create(int orderId)
    {
        var model = await _returnOrderService.GetReturnFormByOrderIdAsync(orderId);
        if (model == null)
        {
            return NotFound("Order not found.");
        }

        return View(model);
    }

    // POST: /ReturnOrder/Create
    [HttpPost]
    public async Task<IActionResult> Create(ReturnOrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var success = await _returnOrderService.ProcessReturnAsync(model);

        if (!success)
        {
            ModelState.AddModelError("", "Something went wrong while processing the return.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Return processed successfully.";
        return RedirectToAction("Index", "Order"); // Or wherever appropriate
    }
}
