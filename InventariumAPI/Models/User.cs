using AutoMapper.Configuration.Annotations;
using Bogus;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

public class User: IGenericModel<TModelId>, IFakerFactory<User>
{
    public TModelId UserId { get; set; }

    [MinLength(4)]
    [StringLength(50)]
    public required string Name { get; set; }

    [NullSubstitute(UserRole.Customer)]
    public UserRole Role { get; set; }
    public virtual ICollection<Lendout> Lendouts { get; set; } = [];

    public static Faker<User> CreateFaker(DataContext context)
        => new Faker<User>()
            .RuleFor(p => p.Name, g => g.Name.FullName())
            .RuleFor(p => p.Role, g => UserRole.Customer);

    public TModelId GetId() => UserId;
    
    public override string ToString() => $"user named '{Name}'(ID {GetId()})";
}

public enum UserRole
{
    Customer, 
    Manager
}