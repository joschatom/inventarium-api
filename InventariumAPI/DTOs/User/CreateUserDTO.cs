using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.User;

public class CreateUserDTO: IBaseDTO<Models.User, TModelId>
{
    [MinLength(4)]
    public required string Name { get; set; }
}
