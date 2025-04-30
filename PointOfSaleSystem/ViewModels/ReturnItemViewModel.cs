using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class ReturnItemViewModel
    {
        public int OrderItemId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int MaxQuantity { get; set; } // From original order item

        public int Quantity { get; set; } // Return quantity entered by user

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }
    }

}

