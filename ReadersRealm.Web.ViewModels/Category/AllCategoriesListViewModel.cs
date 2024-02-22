namespace ReadersRealm.Web.ViewModels.Category;

/// <summary>
/// A ViewModel representing a collection of all categories in the Readers Realm application.
/// This model is used for display all categories as options we can choose from during
/// the creating of a new book.
/// </summary>
public class AllCategoriesListViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    /// <value>
    /// The unique identifier assigned to each category.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    /// <value>
    /// The name of the category. This is used for display purposes in the UI and should be clear and descriptive.
    /// </value>
    public required string Name { get; set; }
}