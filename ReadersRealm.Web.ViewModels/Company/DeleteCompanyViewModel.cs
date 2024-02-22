namespace ReadersRealm.Web.ViewModels.Company;

public class DeleteCompanyViewModel
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string UIC { get; set; }

    public required string Email { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }
}