namespace PointOfSaleSystem.RolesViewModel
{
    public class EditRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleUserViewModel> Users { get; set; } = new();
    }
}
