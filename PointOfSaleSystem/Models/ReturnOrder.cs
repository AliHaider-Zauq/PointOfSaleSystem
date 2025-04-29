namespace PointOfSaleSystem.Models
{
    public class ReturnOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public DateTime ReturnDate { get; set; }

        public ICollection<ReturnOrderItem> ReturnItems { get; set; } = new List<ReturnOrderItem>();
    }

}
