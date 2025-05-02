using PointOfSaleSystem.ViewModels;

namespace PointOfSaleSystem.Services
{
    public interface IReportService
    {
        Task<DashboardIndexViewModel> GetDashboardReportAsync();
    }
}
