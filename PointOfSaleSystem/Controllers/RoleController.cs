using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleSystem.RolesViewModel;


[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "This role already exists.");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();

        var users = _userManager.Users.ToList();

        var model = new EditRoleViewModel
        {
            RoleId = role.Id,
            RoleName = role.Name,
            Users = users.Select(user => new RoleUserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.RoleId);
        if (role == null) return NotFound();

        foreach (var userModel in model.Users)
        {
            var user = await _userManager.FindByIdAsync(userModel.UserId);
            if (user == null) continue;

            if (userModel.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            else if (!userModel.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
        }

        return RedirectToAction("Index");
    }

}
