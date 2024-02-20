namespace ReadersRealm.Common.Exceptions.Company;

public class CompanyNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested company was not found.";

    public CompanyNotFoundException()
        : base(DefaultMessage) { }

    public CompanyNotFoundException(string message)
        : base(message) { }
}