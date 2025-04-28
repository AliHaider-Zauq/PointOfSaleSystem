using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }
    }

}
