using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

[PrimaryKey(nameof(ObjectId), nameof(UserId))]

public class ObjectManager: IGenericModel<(TModelId, TModelId)>
{
    public TModelId ObjectId { get; set; }
    public required virtual ObjectEntry Object { get; set; }

    public TModelId UserId { get; set; }
    public required virtual User User { get; set; }

    public (TModelId, TModelId) GetId() => (ObjectId, UserId);
}
