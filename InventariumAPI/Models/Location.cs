using Bogus;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventariumAPI.Models;

public class Location: IGenericModel<TModelId>, IFakerFactory<Location>
{
    public TModelId LocationId { get; set; }
    [MinLength(1)]
    [MaxLength(50)]
    public required string Name { get; set; }

    public virtual ICollection<ObjectEntry> Objects { get; set; } = [];

    public static Faker<Location> CreateFaker(DataContext context)
        => new Faker<Location>()
            .RuleFor(p => p.Name, g => g.Lorem.Word());

    public TModelId GetId() => LocationId;
}
