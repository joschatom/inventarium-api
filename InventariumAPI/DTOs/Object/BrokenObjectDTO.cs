using InventariumAPI.DTOs.User;
using InventariumAPI.Interfaces;

namespace InventariumAPI.DTOs.Object;

public class BrokenObjectDTO: IBaseDTO<Models.BrokenObject, int>
{
    public required TModelId ObjectId { get; set; }
    public required string Reason { get; set; }
}
