namespace ReadersRealm.DataModels;

using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Range(1, 100)]
    public int DisplayOrder { get; set; }
}