namespace ReadersRealm.ViewModels.ShoppingCart;

public class AllShoppingCartsListViewModel
{
    public AllShoppingCartsListViewModel()
    {
        this.ShoppingCartsList = new HashSet<ShoppingCartViewModel>();
    }

    public decimal OrderTotal { get; set; }

    public IEnumerable<ShoppingCartViewModel> ShoppingCartsList { get; set; }
}