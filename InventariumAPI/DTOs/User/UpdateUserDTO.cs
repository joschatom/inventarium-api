using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.User;

public class UpdateUserDTO: UserDTO
{
    public new string? Name { get; set; }
    [SwaggerIgnore]
    [Ignore]
    public new string? Role { get; set; }
}
