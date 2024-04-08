namespace ReadersRealm.Web.ViewModels.ShoppingCart;

using OrderHeader;

public class AllShoppingCartsListViewModel
{
    public OrderHeaderViewModel OrderHeader { get; set; } = null!;

    public IEnumerable<ShoppingCartViewModel> ShoppingCartsList { get; set; } = new HashSet<ShoppingCartViewModel>();
}