namespace ReadersRealm.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Common.Constants.ValidationConstants.OrderHeader;
public class OrderHeader
{
    public OrderHeader()
    {
        this.Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime ShippingDate { get; set; }

    [Column(TypeName = OrderHeaderOrderTotalColumnType)]
    public decimal OrderTotal { get; set; }

    [StringLength(OrderHeaderOrderStatusMaxLength)]
    public string? OrderStatus { get; set; }

    [StringLength(OrderHeaderPaymentStatusMaxLength)]
    public string? PaymentStatus { get; set; }

    [StringLength(OrderHeaderTrackingNumberMaxLength)]
    public string? TrackingNumber { get; set; }

    [StringLength(OrderHeaderCarrierMaxLength)]
    public string? Carrier { get; set; }

    public DateTime PaymentDate { get; set; }

    public DateOnly PaymentDueDate { get; set; }

    [StringLength(OrderHeaderSessionIdMaxLength)]
    public string? SessionId { get; set; }

    [StringLength(OrderHeaderPaymentIntentIdMaxLength)]
    public string? PaymentIntentId { get; set; }
}