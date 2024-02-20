// ReSharper disable InconsistentNaming

namespace ReadersRealm.ViewModels.Company;

public class AllCompaniesViewModel
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