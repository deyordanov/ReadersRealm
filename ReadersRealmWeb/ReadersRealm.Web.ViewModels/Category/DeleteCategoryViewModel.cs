namespace ReadersRealm.ViewModels.Category;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// ViewModel for deleting a category in the Readers Realm application.
/// This model is used to represent the category that is intended to be deleted.
/// It provides essential information to confirm the deletion action.
/// </summary>
public class DeleteCategoryViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the category to be deleted.
    /// </summary>
    /// <value>
    /// The unique identifier for the category. This is used to ensure that the correct category is targeted for deletion.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category to be deleted.
    /// </summary>
    /// <value>
    /// The name of the category. Displaying the name helps users to confirm that they are deleting the correct category.
    /// </value>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display order of the category to be deleted.
    /// </summary>
    /// <value>
    /// The display order of the category. This information can be used as an additional reference to ensure the correct category is selected for deletion.
    /// </value>
    public int DisplayOrder { get; set; }
}