using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.Object;

public class UpdateObjectDTO: ObjectDTO
{
    [Ignore]
    [SwaggerIgnore]
    public new string? State { get; set; } = null;
    public new string? Name { get; set; } = null;
    public new string? Description { get; set; } = null;
}
