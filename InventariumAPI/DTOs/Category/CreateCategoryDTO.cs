using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Category;

public class CreateCategoryDTO: CategoryDTO
{
    [MinLength(1)]
    public new required string Name { get; set; }
}
