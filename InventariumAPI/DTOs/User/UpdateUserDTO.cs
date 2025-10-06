using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.User;

public class UpdateUserDTO: IBaseDTO<Models.User, TModelId>
{
    public string? Name { get; set; }
}
