using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.User;

public class CreateUserDTO: UserDTO
{
    public new required string Name { get; set; }
    [SwaggerIgnore]
    [Ignore]
    public new string? Role { get; set; } = nameof(UserRole.Customer);
}
