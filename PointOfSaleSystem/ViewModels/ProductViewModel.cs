using PointOfSaleSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SalePrice { get; set; } // Sale price is managed manually or from last purchase

        public int Quantity { get; set; } // This is computed from purchase batches

        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Supplier>? Suppliers { get; set; }
    }

}
