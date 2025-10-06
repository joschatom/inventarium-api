using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.User;

public class CreateUserDTO: IBaseDTO<Models.User, TModelId>
{
    public required string Name { get; set; }
}
