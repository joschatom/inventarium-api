
#nullable disable
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Location;
public class LocationDTO : IBaseDTO<Models.Location, int>, IDtoTypes
{
    public static Type CreateDTO => typeof(CreateLocationDTO);
    public static Type UpdateDTO => typeof(UpdateLocationDTO);
    public static Type BaseDTO => typeof(LocationDTO);
    public static Type BaseType => typeof(Models.Location);

    public string Name { get; set; }
}

