namespace ReadersRealm.ViewModels.OrderHeader;

using Data.Models;
using Web.ViewModels.OrderDetails;

public class OrderHeaderReceiptViewModel
{
    public Guid Id { get; set; }

    public required ApplicationUser ApplicationUser { get; set; }

    public required DateTime OrderDate { get; set; }

    public required decimal OrderTotal { get; set; }

    public required string PaymentStatus { get; set; }

    public required DateTime PaymentDate { get; set; }

    public required string PaymentIntentId { get; set; }

    public required IEnumerable<OrderDetailsViewModel> OrderDetails { get; set; }
}