namespace ReadersRealm.Common.Exceptions.OrderHeader;

public class OrderHeaderNotFoundException : BaseNotFoundException
{
    private const string DefaultMessage = "The requested order header was not found.";

    public OrderHeaderNotFoundException()
        : base(DefaultMessage) { }

    public OrderHeaderNotFoundException(string message)
        : base(message) { }
}