namespace ReadersRealm.Common.Exceptions.OrderHeader;

public class OrderHeaderNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested order header was not found.";

    public OrderHeaderNotFoundException()
        : this(DefaultMessage) { }
}