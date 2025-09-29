using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

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

    [ValueConverter(typeof(StringToEnumConverter<ObjectState>))]
    public string State { get; set; }
}

