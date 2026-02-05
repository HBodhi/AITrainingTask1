using MomentusEventApi.Models;

namespace MomentusEventApi.Services;

public interface IServiceOrderService
{
    Task<IEnumerable<ServiceOrder>> GetAllOrdersAsync(int eventId);
    Task<ServiceOrder?> GetOrderByNumberAsync(int orderNumber);
    Task<IEnumerable<ServiceOrder>> SearchOrdersAsync(string searchFilter);
}
