using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class PurchaseItemViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? SalePrice { get; set; } // Optional
    }

}
