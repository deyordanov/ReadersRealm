﻿namespace ReadersRealm.Data.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.Constants.ValidationConstants.OrderHeaderValidation;
public class OrderHeader
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; } = null!;

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

    public Order Order { get; set; } = null!;
}