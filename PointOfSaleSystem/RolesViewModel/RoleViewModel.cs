using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.RolesViewModel
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
