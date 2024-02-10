namespace ReadersRealm.ViewModels.Order;

using OrderDetails;
using ReadersRealm.ViewModels.OrderHeader;

public class OrderViewModel
{
    public OrderViewModel()
    {
        this.OrderDetailsList = new HashSet<OrderDetailsViewModel>();
    }

    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    public OrderHeaderViewModel OrderHeader { get; set; } = null!;

    public IEnumerable<OrderDetailsViewModel> OrderDetailsList { get; set; }
}