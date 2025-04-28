namespace PointOfSaleSystem.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

}
