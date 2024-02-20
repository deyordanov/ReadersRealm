namespace ReadersRealm.Common.Exceptions.EntityProperty;

public class PropertyNotFoundException : Exception
{
    private const string DefaultMessage = "One (or more) of the listed properties was not present in the entity!";

    public PropertyNotFoundException()
    : base(DefaultMessage) { }

    public PropertyNotFoundException(string message)
        : base(message) { }
}