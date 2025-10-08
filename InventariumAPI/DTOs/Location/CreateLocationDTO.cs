using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Location;

public class CreateLocationDTO: LocationDTO
{
    [MinLength(1)]
    public new required string Name { get; set; }
}
