using Microsoft.EntityFrameworkCore;

namespace ReadersRealm.ViewModels.Category;

/// <summary>
/// A ViewModel representing a collection of all categories in the Readers Realm application.
/// This model is typically used for displaying categories in user interfaces where
/// a list of categories is required.
/// </summary>
[Comment("All Categories View Model")]
public class AllCategoriesViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    /// <value>
    /// The unique identifier assigned to each category.
    /// </value>
    [Comment("All Categories View Model Identifier")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    /// <value>
    /// The name of the category. This is used for display purposes in the UI and should be clear and descriptive.
    /// </value>
    [Comment("All Categories View Model Name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display order of the category.
    /// </summary>
    /// <value>
    /// An integer representing the display order of the category. Categories with lower display orders are typically shown before those with higher orders.
    /// </value>
    [Comment("All Categories View Model Display Order")]
    public int DisplayOrder { get; set; }
}