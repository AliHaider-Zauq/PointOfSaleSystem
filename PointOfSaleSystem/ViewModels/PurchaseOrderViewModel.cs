using PointOfSaleSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class PurchaseOrderViewModel
    {
        [Required(ErrorMessage = "Please select a supplier")]
        public int SupplierId { get; set; }

        public List<PurchaseItemViewModel> PurchaseItems { get; set; } = new();

        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Supplier>? Suppliers { get; set; }
    }

}
