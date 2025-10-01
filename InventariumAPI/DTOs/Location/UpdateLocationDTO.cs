using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.Location;

public class UpdateLocationDTO : LocationDTO
{
    public new string? Name { get; set; }
}
