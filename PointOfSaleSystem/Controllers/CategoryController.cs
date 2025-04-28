using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.Models;
using PointOfSaleSystem.Services;
using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _service.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _service.AddAsync(new Category { Name = vm.Name });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(new CategoryViewModel { Id = category.Id, Name = category.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _service.UpdateAsync(new Category { Id = vm.Id, Name = vm.Name });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsCategoryNameAvailable(string name)
        {
            bool exists = await _service.CategoryNameExistsAsync(name);
            return Json(!exists); // true = valid, false = already exists
        }

    }
}