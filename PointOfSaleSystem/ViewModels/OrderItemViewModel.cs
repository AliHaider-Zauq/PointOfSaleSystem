using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class OrderItemViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

}
