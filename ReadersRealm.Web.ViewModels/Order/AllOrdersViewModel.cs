﻿namespace ReadersRealm.Web.ViewModels.Order;

using OrderDetails;
using OrderHeader;

public class AllOrdersViewModel
{
    public AllOrdersViewModel()
    {
        OrderDetailsList = new HashSet<OrderDetailsViewModel>();
    }

    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    public OrderHeaderViewModel OrderHeader { get; set; } = null!;

    public IEnumerable<OrderDetailsViewModel> OrderDetailsList { get; set; }
}