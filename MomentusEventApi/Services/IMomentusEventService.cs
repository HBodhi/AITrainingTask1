using MomentusEventApi.Models;

namespace MomentusEventApi.Services;

public interface IMomentusEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event?> GetEventByIdAsync(string id);
}
