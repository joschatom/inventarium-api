using InventariumAPI.Interfaces;

namespace InventariumAPI.Models;

public class Category: IGenericModel<TModelId>
{
    public TModelId CategoryId { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<ObjectEntry> Objects { get; set; } = [];

    public TModelId GetId() => CategoryId;
}
