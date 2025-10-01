using InventariumAPI.Interfaces;

namespace InventariumAPI.Models;

public class Location: IGenericModel<TModelId>
{
    public TModelId LocationId { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<ObjectEntry> Objects { get; set; } = [];

    public TModelId GetId() => LocationId;
}
