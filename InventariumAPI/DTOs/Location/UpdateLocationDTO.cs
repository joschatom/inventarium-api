using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Location;

public class UpdateLocationDTO : LocationDTO
{
    [MinLength(1)]
    public new string? Name { get; set; }
}
