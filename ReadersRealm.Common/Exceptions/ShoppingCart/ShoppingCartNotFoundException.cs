namespace ReadersRealm.Common.Exceptions.ShoppingCart;

public class ShoppingCartNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested shopping cart was not found.";

    public ShoppingCartNotFoundException()
        : this(DefaultMessage) { }
}