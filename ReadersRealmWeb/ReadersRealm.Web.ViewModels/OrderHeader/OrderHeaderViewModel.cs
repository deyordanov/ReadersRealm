﻿namespace ReadersRealm.ViewModels.OrderHeader;

using ApplicationUser;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.Constants.ValidationConstants.ApplicationUser;
using static Common.Constants.ValidationMessageConstants.ApplicationUser;

public class OrderHeaderViewModel
{
    public Guid Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public OrderApplicationUserViewModel ApplicationUser { get; set; }

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

    [StringLength(ApplicationUserFirstNameMaxLength,
        MinimumLength = ApplicationUserFirstNameMinLength,
        ErrorMessage = ApplicationUserFirstNameLengthMessage)]
    public required string FirstName { get; set; }

    [StringLength(ApplicationUserLastNameMaxLength,
        MinimumLength = ApplicationUserLastNameMinLength,
        ErrorMessage = ApplicationUserLastNameLengthMessage)]
    public required string LastName { get; set; }

    [StringLength(ApplicationUserStreetAddressMaxLength,
        MinimumLength = ApplicationUserStreetAddressMinLength,
        ErrorMessage = ApplicationUserStreetAddressLengthMessage)]
    public required string StreetAddress { get; set; }

    [StringLength(ApplicationUserCityMaxLength,
        MinimumLength = ApplicationUserCityMinLength,
        ErrorMessage = ApplicationUserCityLengthMessage)]
    public required string City { get; set; }

    [StringLength(ApplicationUserStateMaxLength,
        MinimumLength = ApplicationUserStateMinLength,
        ErrorMessage = ApplicationUserStateLengthMessage)]
    public required string State { get; set; }

    [StringLength(ApplicationUserPostalCodeMaxLength,
        MinimumLength = ApplicationUserPostalCodeMinLength,
        ErrorMessage = ApplicationUserPostalCodeLengthMessage)]
    public required string PostalCode { get; set; }

    [StringLength(ApplicationUserPhoneNumberMaxLength,
        MinimumLength = ApplicationUserPhoneNumberMinLength,
        ErrorMessage = ApplicationUserPhoneNumberLengthMessage)]
    public required string PhoneNumber { get; set; }
}