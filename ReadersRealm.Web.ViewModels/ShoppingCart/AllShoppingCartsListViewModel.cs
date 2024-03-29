﻿namespace ReadersRealm.Web.ViewModels.ShoppingCart;

using OrderHeader;

public class AllShoppingCartsListViewModel
{
    public AllShoppingCartsListViewModel()
    {
        ShoppingCartsList = new HashSet<ShoppingCartViewModel>();
    }

    public OrderHeaderViewModel OrderHeader { get; set; } = null!;

    public IEnumerable<ShoppingCartViewModel> ShoppingCartsList { get; set; }
}