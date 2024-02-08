namespace ReadersRealm.Common.Exceptions;

public class OrderHeaderNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested order header was not found.";

    public OrderHeaderNotFoundException()
        : base(DefaultMessage) { }

    public OrderHeaderNotFoundException(string message)
        : base(message) { }
}