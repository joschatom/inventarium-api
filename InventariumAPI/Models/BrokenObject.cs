using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;


public class BrokenObject: IGenericModel<TModelId>
{
    [Key]
    public TModelId ObjectId { get; set; }
    public virtual ObjectEntry Object { get; set; } = null!;

    [MinLength(8)]
    public required string Reason { get; set; }

    public TModelId GetId() => ObjectId;
}
