using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Services.Interfaces;
using PointOfSaleSystem.ViewModels;
using PointOfSaleSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PointOfSaleSystem.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _service.GetAllAsync();
            return View(customers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var customer = new Customer
            {
                Name = vm.Name
            };

            await _service.AddAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var vm = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var customer = new Customer
            {
                Id = vm.Id,
                Name = vm.Name
            };

            await _service.UpdateAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
