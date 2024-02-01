namespace ReadersRealm.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Contracts;

/// <summary>
/// Represents a category in Readers Realm.
/// Categories are used to organize books or articles into different sections based on their genre or topic.
/// </summary>
/// <remarks>
/// The DisplayOrder property determines the order in which categories are shown in the UI.
/// It must be within the range of 1 to 100, where 1 is the highest priority.
/// </remarks>
/// <example>
/// Example usage:
/// Category fictionCategory = new Category { Name = "Fiction", DisplayOrder = 1 };
/// </example>
[Comment("Readers Realm Category")]
public class Category : IReadersRealmDbContextBaseEntityModel<int>
{
    /// <summary>
    /// Gets or sets the unique identifier for the Category.
    /// </summary>
    /// <value>The Category's unique identifier.</value>
    [Key]
    [Comment("Category Identifier")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the Category.
    /// This name is displayed in the user interface and should be unique.
    /// </summary>
    /// <value>The name of the Category.</value>
    [Required]
    [StringLength(50)]
    [Comment("Category Name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display order of the Category.
    /// Categories with lower display orders are shown first.
    /// </summary>
    /// <value>The display order of the Category.</value>
    [Range(1, 100)]
    [Comment("Category Display Order")]
    public int DisplayOrder { get; set; }
}