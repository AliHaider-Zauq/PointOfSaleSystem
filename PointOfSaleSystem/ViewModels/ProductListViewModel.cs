namespace PointOfSaleSystem.ViewModels
{
    public class ProductListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; } // From remaining in batches

        public decimal SalePrice { get; set; }

        public decimal? LatestPurchasePrice { get; set; } // Only for dashboard

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }
    }

}
