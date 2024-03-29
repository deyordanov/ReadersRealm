﻿namespace ReadersRealm.Web.ViewModels.Category;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Common.Constants.Constants.CategoryConstants;
using static Common.Constants.ValidationConstants.CategoryValidation;
using static Common.Constants.ValidationMessageConstants.CategoryValidationMessages;

/// <summary>
/// ViewModel for creating a new category in the Readers Realm application.
/// This model is used to gather information from the user necessary to create a new category.
/// It includes validation rules to ensure the data received is valid.
/// </summary>
public class CreateCategoryViewModel
{
    /// <summary>
    /// Gets or sets the name of the category to be created.
    /// </summary>
    /// <value>
    /// The name of the category. This is a required field and must meet specified length constraints.
    /// </value>
    /// <remarks>
    /// The name must be between <see cref="CategoryNameMinLength"/> and <see cref="CategoryNameMaxLength"/> characters.
    /// This is enforced through validation attributes and any attempts to submit a name outside of these constraints
    /// will result in a <see cref="CategoryNameLengthMessage"/>.
    /// </remarks>
    [Required(ErrorMessage = CategoryNameRequiredMessage)]
    [DisplayName(CategoryName)]
    [StringLength(CategoryNameMaxLength,
        ErrorMessage = CategoryNameLengthMessage,
        MinimumLength = CategoryNameMinLength)]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display order for the category to be created.
    /// </summary>
    /// <value>
    /// An integer representing the display order of the category. This is used to determine the order in which
    /// categories are displayed to the user.
    /// </value>
    /// <remarks>
    /// The display order must be within the range defined by <see cref="CategoryDisplayOrderMinRange"/> and <see cref="CategoryDisplayOrderMaxRange"/>.
    /// Attempts to submit a value outside this range will result in a <see cref="CategoryDisplayOrderRangeMessage"/>.
    /// </remarks>
    [DisplayName(CategoryDisplayOrder)]
    [Range(CategoryDisplayOrderMinRange,
        CategoryDisplayOrderMaxRange,
        ErrorMessage = CategoryDisplayOrderRangeMessage)]
    public int DisplayOrder { get; set; }
}
