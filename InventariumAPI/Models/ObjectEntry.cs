using AutoMapper.Configuration.Annotations;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Object;
using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace InventariumAPI.Models;

public enum ObjectState
{
    Lendout,
    Free,
    Broken
}

public class ObjectEntry: IGenericModel<TModelId>
{
    [Key]
    public TModelId ObjectId { get; set; }

    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; } = "Unnamed";

    [MaxLength(255)]
    [MinLength(1)]
    public string Description { get; set; } = "N/A";

    public TModelId LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;

    public TModelId CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ObjectManager> Managers { get; set; } = [];
    public Lendout? Lendout { get; set; }

    public ObjectState State =>
        Lendout is null
            ? ObjectState.Free
            : ObjectState.Lendout;


    public int GetId() => ObjectId;
}
