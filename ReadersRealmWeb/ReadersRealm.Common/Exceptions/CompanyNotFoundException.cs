namespace ReadersRealm.Common.Exceptions;

public class CompanyNotFoundException : ApplicationException
{
    private const string DefaultMessage = "Such a company was not found!";

    public CompanyNotFoundException()
        : base(DefaultMessage) { }

    public CompanyNotFoundException(string message)
        : base(message) { }
}