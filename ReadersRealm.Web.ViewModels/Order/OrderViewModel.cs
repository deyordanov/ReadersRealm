namespace ReadersRealm.Web.ViewModels.Order;

using OrderDetails;
using OrderHeader;

public class OrderViewModel
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    public OrderHeaderViewModel OrderHeader { get; set; } = null!;

    public IEnumerable<OrderDetailsViewModel> OrderDetailsList { get; set; } = new HashSet<OrderDetailsViewModel>();
}