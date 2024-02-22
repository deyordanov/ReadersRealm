namespace ReadersRealm.Web.ViewModels.ApplicationUser;

public class OrderApplicationUserViewModel
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = string.Empty;
}