using System.ComponentModel.DataAnnotations;
namespace PointOfSaleSystem.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }

        [Display(Name = "Contact Info")]
        public string ContactInfo { get; set; }
    }

}
