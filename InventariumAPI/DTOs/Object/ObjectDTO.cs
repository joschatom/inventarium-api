using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
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

    public int LocationId { get; set; }
    public int CategoryId { get; set; }

    [ValueConverter(typeof(EnumToStringConverter<ObjectState>))]
    public string State { get; set; }
}