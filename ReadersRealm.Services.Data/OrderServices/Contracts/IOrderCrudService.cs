namespace ReadersRealm.Services.Data.OrderServices.Contracts;

using Web.ViewModels.Order;

public interface IOrderCrudService
{
    Task UpdateOrderAsync(DetailsOrderViewModel orderModel);
}