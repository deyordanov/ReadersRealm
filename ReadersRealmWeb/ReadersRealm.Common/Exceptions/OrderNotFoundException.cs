namespace ReadersRealm.Common.Exceptions;

public class OrderNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested order was not found!";

    public OrderNotFoundException()
        : base(DefaultMessage) { }

    public OrderNotFoundException(string message)
        : base(message) { }
}