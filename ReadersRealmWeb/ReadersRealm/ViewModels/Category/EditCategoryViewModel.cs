using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReadersRealm.ViewModels.Category;
using static ReadersRealm.Common.Constants.Category;
using static ReadersRealm.Common.ValidationMessages.Category;
using static ReadersRealm.Common.ValidationConstants.Category;

public class EditCategoryViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = CategoryNameRequiredMessage)]
    [DisplayName(CategoryName)]
    [StringLength(CategoryNameMaxLength,
        ErrorMessage = CategoryNameLengthMessage,
        MinimumLength = CategoryNameMinLength)]
    public string Name { get; set; } = null!;

    [DisplayName(CategoryDisplayOrder)]
    [Range(CategoryDisplayOrderMinRange,
        CategoryDisplayOrderMaxRange,
        ErrorMessage = CategoryDisplayOrderRangeMessage)]
    public int DisplayOrder { get; set; }
}