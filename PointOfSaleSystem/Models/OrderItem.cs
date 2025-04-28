namespace PointOfSaleSystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal ProductPurchasePrice { get; set; }   
        public decimal ProductSalePrice { get; set; }       
        public string ProductName { get; set; }
    }

}
