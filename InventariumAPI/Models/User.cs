using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Models;

public class User: IGenericModel<TModelId>
{
    public TModelId UserId { get; set; }

    [MinLength(4)]
    [StringLength(50)]
    public required string Name { get; set; }

    [NullSubstitute(UserRole.Customer)]
    public UserRole Role { get; set; }
    public virtual ICollection<Lendout> Lendouts { get; set; } = [];

    public TModelId GetId() => UserId;
}

public enum UserRole
{
    Customer, 
    Manager



}