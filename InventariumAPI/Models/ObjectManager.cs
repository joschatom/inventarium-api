using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

[PrimaryKey(nameof(ObjectId), nameof(UserId))]

public class ObjectManager: IGenericModel<(TModelId, TModelId)>
{
    public TModelId ObjectId { get; set; }
    public virtual ObjectEntry Object { get; set; } = null!;

    public TModelId UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public (TModelId, TModelId) GetId() => (ObjectId, UserId);
}
