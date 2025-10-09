using Bogus;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

public class Category: IGenericModel<TModelId>, IFakerFactory<Category>
{
    public TModelId CategoryId { get; set; }
    [StringLength(50)]
    [MinLength(1)]
    public required string Name { get; set; }

    public virtual ICollection<ObjectEntry> Objects { get; set; } = [];

    public static Faker<Category> CreateFaker(DataContext context)
        => new Faker<Category>()
            .RuleFor(p => p.Name, g => g.Lorem.Word());

    public TModelId GetId() => CategoryId;
}
