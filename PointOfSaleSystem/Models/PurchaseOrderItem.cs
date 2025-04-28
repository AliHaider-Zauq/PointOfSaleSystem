namespace PointOfSaleSystem.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }                     // Purchased
        public int RemainingQuantity { get; set; }            // Left from batch
        public decimal PurchasePrice { get; set; }
    }
}
