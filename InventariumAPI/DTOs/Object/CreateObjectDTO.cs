using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Object;

public class CreateObjectDTO: ObjectDTO
{
    public new required string Name { get; set; }
    public new required string Description { get; set; }

    [ValueConverter(typeof(StringToEnumConverter<ObjectState>))]
    public new required string State { get; set; }
}
