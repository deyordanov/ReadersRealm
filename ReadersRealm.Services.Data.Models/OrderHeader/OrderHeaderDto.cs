namespace ReadersRealm.Services.Data.Models.OrderHeader;

using ApplicationUser;

public class OrderHeaderDto
{
    public Guid Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;

    public SummaryApplicationUserDto ApplicationUser { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public DateTime ShippingDate { get; set; }

    public decimal OrderTotal { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TrackingNumber { get; set; }

    public string? Carrier { get; set; }

    public DateTime PaymentDate { get; set; }

    public DateOnly PaymentDueDate { get; set; }

    public string? PaymentIntentId { get; set; }

    public string? SessionId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string StreetAddress { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }

    public required string PostalCode { get; set; }

    public required string PhoneNumber { get; set; }
}