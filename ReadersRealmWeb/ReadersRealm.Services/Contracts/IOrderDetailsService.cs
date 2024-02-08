using ReadersRealm.Web.ViewModels.OrderDetails;

namespace ReadersRealm.Services.Contracts;

public interface IOrderDetailsService
{
    Task CreateOrderDetailsAsync(OrderDetailsViewModel orderDetailsModel);
}