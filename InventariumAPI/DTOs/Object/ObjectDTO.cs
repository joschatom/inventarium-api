using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using InventariumAPI.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace InventariumAPI.DTOs.Object;

#nullable disable
public class ObjectDTO : IBaseDTO<Models.ObjectEntry, int>, IDtoTypes
{
    public static Type CreateDTO => typeof(CreateObjectDTO);
    public static Type UpdateDTO => typeof(UpdateObjectDTO);
    public static Type BaseDTO => typeof(ObjectDTO);
    public static Type BaseType => typeof(Models.ObjectEntry);

    public string Name { get; set; }
    public string Description { get; set; }

    [ReferenceToModel(typeof(Models.Location), typeof(TModelId))]
    public int LocationId { get; set; }
    [ReferenceToModel(typeof(Models.Category), typeof(TModelId))]
    public int CategoryId { get; set; }

    [ValueConverter(typeof(EnumToStringConverter<ObjectState>))]
    public string State { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        return [];
        var dataContext = validationContext.GetService<DataContext>();
        var self = (ObjectDTO)validationContext.ObjectInstance;

        var locationErr = dataContext.Locations.Find(self.LocationId) is null
            ? new ValidationResult(
                $"Location with ID {self.LocationId} doesn't exist.", 
                [nameof(LocationId)]
            ) : null;
            
        var categoryErr = dataContext.Categories.Find(CategoryId) is null
            ? new ValidationResult(
                $"Category with ID {CategoryId} doesn't exist.",
                [nameof(CategoryId)]
            ) : null;

        if (locationErr != null && categoryErr != null)
            return [locationErr, categoryErr];
        else if (locationErr != null)
            return [locationErr];
        else if (categoryErr != null)
            return [categoryErr];
        else return [];
    }
}

