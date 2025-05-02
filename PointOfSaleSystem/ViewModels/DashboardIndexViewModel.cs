namespace PointOfSaleSystem.ViewModels
{
    public class DashboardIndexViewModel
    {
        public IEnumerable<ProductListViewModel> Products { get; set; }
        public decimal TotalSalesToday { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public List<TopCustomerViewModel> TopCustomers { get; set; }
        public List<TopProductViewModel> TopProducts { get; set; }
        public List<ProductListViewModel> LowStockProducts { get; set; }
    }
}
