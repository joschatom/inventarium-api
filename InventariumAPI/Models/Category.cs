using InventariumAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

public class Category: IGenericModel<TModelId>
{
    public TModelId CategoryId { get; set; }
    [StringLength(50)]
    [MinLength(1)]
    public required string Name { get; set; }

    public virtual ICollection<ObjectEntry> Objects { get; set; } = [];

    public TModelId GetId() => CategoryId;
}
