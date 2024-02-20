namespace ReadersRealm.ViewModels.Category;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ReadersRealm.Common.Constants.Constants.Category;
using static ReadersRealm.Common.Constants.ValidationConstants.Category;
using static ReadersRealm.Common.Constants.ValidationMessageConstants.Category;

/// <summary>
/// ViewModel for editing an existing category in the Readers Realm application.
/// This model is used to capture and validate user input when updating category details.
/// It includes validation rules to ensure the updated data is valid.
/// </summary>
public class EditCategoryViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the category being edited.
    /// </summary>
    /// <value>
    /// The unique identifier for the category. This is crucial for identifying which category is being updated.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category being edited.
    /// </summary>
    /// <value>
    /// The name of the category. This field is required and must adhere to specified length constraints.
    /// </value>
    /// <remarks>
    /// The name must be between <see cref="CategoryNameMinLength"/> and <see cref="CategoryNameMaxLength"/> characters.
    /// This constraint is enforced through validation attributes, and an error message <see cref="CategoryNameLengthMessage"/>
    /// will be displayed for invalid submissions.
    /// </remarks>
    [Required(ErrorMessage = CategoryNameRequiredMessage)]
    [DisplayName(CategoryName)]
    [StringLength(CategoryNameMaxLength,
        ErrorMessage = CategoryNameLengthMessage,
        MinimumLength = CategoryNameMinLength)]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display order of the category being edited.
    /// </summary>
    /// <value>
    /// An integer representing the display order of the category. Categories are displayed based on this order.
    /// </value>
    /// <remarks>
    /// The display order must be within the range defined by <see cref="CategoryDisplayOrderMinRange"/> and <see cref="CategoryDisplayOrderMaxRange"/>.
    /// Attempts to submit a value outside this range will be flagged with an error message <see cref="CategoryDisplayOrderRangeMessage"/>.
    /// </remarks>
    [DisplayName(CategoryDisplayOrder)]
    [Range(CategoryDisplayOrderMinRange,
        CategoryDisplayOrderMaxRange,
        ErrorMessage = CategoryDisplayOrderRangeMessage)]
    public int DisplayOrder { get; set; }
}
