namespace PointOfSaleSystem.Models
{
    public class ReturnOrderItem
    {
        public int Id { get; set; }
        public int ReturnId { get; set; }
        public ReturnOrder ReturnOrder { get; set; } = null!;

        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;

        public int Quantity { get; set; }

        // Redundant info for reporting/stability
        public decimal PurchasePrice { get; set; }     // From original batch
        public decimal SalePrice { get; set; }         // What it was sold for
    }


}
