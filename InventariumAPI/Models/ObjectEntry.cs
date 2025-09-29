using InventariumAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

public class ObjectEntry: IGenericModel<int>, IBaseDTO<ObjectEntry,  int>
{
    [Key]
    public int ObjectId { get; set; }
    public string Name { get; set; } = "Unnamed";
    public string Description { get; set; } = "N/A";
    public ObjectState State { get; set; }

    public int GetId() => ObjectId;
}
