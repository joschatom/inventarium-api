using InventariumAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

public class Location: IGenericModel<TModelId>
{
    public TModelId LocationId { get; set; }
    [MinLength(1)]
    [MaxLength(50)]
    public required string Name { get; set; }
    public virtual ICollection<ObjectEntry> Objects { get; set; } = [];

    public TModelId GetId() => LocationId;
}
