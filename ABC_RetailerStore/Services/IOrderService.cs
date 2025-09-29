using ABCRetail.Web.Models;

namespace ABCRetail.Web.Services
{
    public interface IOrderService
    {
        Task ProcessOrderAsync(OrderMessage orderMessage);
        Task<IEnumerable<string>> GetOrderMessagesAsync();
    }
}
