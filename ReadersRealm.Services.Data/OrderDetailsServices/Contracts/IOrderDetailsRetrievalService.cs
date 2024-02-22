namespace ReadersRealm.Services.Data.OrderDetailsServices.Contracts;

using Models.OrderDetails;
using Web.ViewModels.OrderDetails;

public interface IOrderDetailsRetrievalService
{
    Task<IEnumerable<OrderDetailsViewModel>> GetAllByOrderHeaderIdAsync(Guid orderHeaderId);
    Task<IEnumerable<OrderDetailsReceiptDto>> GetAllOrderDetailsForReceiptAsDtosAsync(Guid orderHeaderId);
}