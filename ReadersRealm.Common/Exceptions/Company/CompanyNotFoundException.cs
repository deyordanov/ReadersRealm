namespace ReadersRealm.Common.Exceptions.Company;

public class CompanyNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested company was not found.";

    public CompanyNotFoundException()
        : this(DefaultMessage) { }
}