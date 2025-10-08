

using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace InventariumAPI.Models;


[PrimaryKey(nameof(ObjectId), nameof(UserId))]
public class Lendout: IGenericModel<(TModelId ObjectId, TModelId UserId)>
{
    public static readonly TimeSpan MINIMUM_DURATION = TimeSpan.FromSeconds(1);

    public TModelId ObjectId { get; set; }
    public TModelId UserId { get; set; }
    public virtual ObjectEntry Object { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    public (TModelId, TModelId) GetId() => (ObjectId, UserId);

    public static object[] DeconstructId((TModelId ObjectId, TModelId UserId) id) => [id.ObjectId, id.UserId];
}

