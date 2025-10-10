using AutoMapper.Configuration.Annotations;
using Bogus;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Object;
using InventariumAPI.Interfaces;
using InventariumAPI.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace InventariumAPI.Models;

public enum ObjectState
{
    Lendout,
    Free,
    Broken,
    BrokenLendout,
}

public class ObjectEntry: IGenericModel<TModelId>, IFakerFactory<ObjectEntry>
{
    [Key]
    public TModelId ObjectId { get; set; }

    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; } = "Unnamed";

    [MaxLength(255)]
    [MinLength(1)]
    public string Description { get; set; } = "N/A";

    [DeniedValues(0)]
    public TModelId LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;

    [DeniedValues(0)]

    public TModelId CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ObjectManager> Managers { get; set; } = [];
    public Lendout? Lendout { get; set; }
    public BrokenObject? BrokenObject { get; set; }

    public ObjectState State => (Lendout != null, BrokenObject != null) switch{
        (false, false) => ObjectState.Free,
        (false, true) => ObjectState.Broken,
        (true, false) => ObjectState.Lendout,
        (_, _) => ObjectState.BrokenLendout
    };

    public static Faker<ObjectEntry> CreateFaker(DataContext context)
        => new Faker<ObjectEntry>()
            .RuleFor(p => p.Name, g => g.Commerce.ProductName())
            .RuleFor(p => p.Description, g => g.Commerce.ProductDescription())
            .RuleFor(p => p.CategoryId, g => g.PickRandom(context.Categories.AsEnumerable()).CategoryId)
            .RuleFor(p => p.LocationId, g => g.PickRandom(context.Locations.AsEnumerable()).LocationId);

    public int GetId() => ObjectId;

    public override string ToString() => $"object named '{Name}'(ID {GetId()})";
}
