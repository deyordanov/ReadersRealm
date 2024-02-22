namespace ReadersRealm.Services.Data.Models.Category;

public class CategoryDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int DisplayOrder { get; set; }
}