namespace ReadersRealm.Common.Exceptions.Order;

public class OrderNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested order was not found!";

    public OrderNotFoundException()
        : this(DefaultMessage) { }
}