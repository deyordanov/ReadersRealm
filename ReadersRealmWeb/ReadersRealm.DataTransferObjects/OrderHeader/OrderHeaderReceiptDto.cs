namespace ReadersRealm.DataTransferObjects.OrderHeader;

using OrderDetails;

public class OrderHeaderReceiptDto
{
    public Guid Id { get; set; }

    public required DateTime OrderDate { get; set; }

    public required decimal OrderTotal { get; set; }

    public required string PaymentStatus { get; set; }

    public required DateTime PaymentDate { get; set; }

    public required string PaymentIntentId { get; set; }

    public required IEnumerable<OrderDetailsReceiptDto> OrderDetails { get; set; }
}