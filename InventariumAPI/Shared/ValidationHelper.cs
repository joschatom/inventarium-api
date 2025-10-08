using AutoMapper.Internal;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Category;
using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Security.AccessControl;

namespace InventariumAPI.Shared;

public static class ValidationHelpers
{
    public static ValidationResult? MustBeNull(this object? _self, ValidationContext validationContext)
        => _self is null
            ? ValidationResult.Success
            : new($"The {validationContext.DisplayName} memeber must be null");

  
    public static ValidationResult? ExistsInDatabase(this object _self, ValidationContext validationContext)
    {
        if (_self is null)
            return ValidationResult.Success;
        if (validationContext.MemberName is null)
            throw new ArgumentNullException(nameof(validationContext));

        var ty = validationContext.ObjectType
            .BaseType == typeof(object)
            ? validationContext.ObjectType
            : validationContext.ObjectType.BaseType!;

        var reftos = ty
            .GetMember(validationContext.MemberName)
            .Select(m => m.GetCustomAttribute<ReferenceToModelAttribute>(true));

        var refto = reftos.FirstOrDefault(m => m is not null)
                ?? throw new InvalidOperationException(
                    $"The member {validationContext.DisplayName} is not marked as a reference.");

        if (_self.GetType() != refto.TargetIdType)
            throw new Exception($"Member type {_self.GetType()} is not the same a target type {refto.TargetIdType}");

        var dbContext = validationContext.GetService<DataContext>()
            ?? throw new Exception("Failed to aquire Database Context.");

        if (dbContext.Find(refto.TargetType, _self) is null)
            return new($"{refto.TargetType.Name} with ID {_self} doesn't exists.", [validationContext.MemberName!]);

        return ValidationResult.Success;
    }
}

[System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class ReferenceToModelAttribute: Attribute
{
    public readonly Type TargetType;
    public readonly Type TargetIdType;

    public ReferenceToModelAttribute(Type model, Type id)
    {
        if (!model.IsAssignableTo(typeof(IGenericModel<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(id)))
            throw new ArgumentException(id.ToString());

        TargetType = model;
        TargetIdType = id;
    }


}
