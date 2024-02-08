namespace ReadersRealm.ViewModels.ShoppingCart;

using Data.Models;
using ReadersRealm.ViewModels.OrderHeader;

public class AllShoppingCartsListViewModel
{
    public AllShoppingCartsListViewModel()
    {
        this.ShoppingCartsList = new HashSet<ShoppingCartViewModel>();
    }

    public OrderHeaderViewModel OrderHeader { get; set; }

    public IEnumerable<ShoppingCartViewModel> ShoppingCartsList { get; set; }
}