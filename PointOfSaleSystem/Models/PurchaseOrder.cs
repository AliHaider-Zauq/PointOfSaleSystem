namespace PointOfSaleSystem.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public DateTime OrderDate { get; set; }

        public List<PurchaseOrderItem> PurchaseItems { get; set; }
    }
}
