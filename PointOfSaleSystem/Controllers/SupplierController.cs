using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.Services;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _service;

        public SupplierController(ISupplierService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _service.GetAllAsync();
            return View(suppliers);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var supplier = new Supplier
            {
                Name = vm.Name,
                ContactInfo = vm.ContactInfo
            };

            await _service.AddAsync(supplier);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _service.GetByIdAsync(id);
            if (supplier == null) return NotFound();

            var vm = new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactInfo = supplier.ContactInfo
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(SupplierViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var supplier = new Supplier
            {
                Id = vm.Id,
                Name = vm.Name,
                ContactInfo = vm.ContactInfo
            };

            await _service.UpdateAsync(supplier);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
