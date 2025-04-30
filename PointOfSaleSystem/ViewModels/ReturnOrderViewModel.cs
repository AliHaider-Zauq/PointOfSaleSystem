using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class ReturnOrderViewModel
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public List<ReturnItemViewModel> ReturnItems { get; set; } = new();
    }
}
