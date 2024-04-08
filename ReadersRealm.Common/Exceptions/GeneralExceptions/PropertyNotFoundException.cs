namespace ReadersRealm.Common.Exceptions.GeneralExceptions;

public class PropertyNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "One (or more) of the listed properties was not present in the entity!";

    public PropertyNotFoundException()
    : this(DefaultMessage) { }
}