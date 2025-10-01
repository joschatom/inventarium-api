
#nullable disable
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.User;
public class UserDTO : IBaseDTO<Models.User, int>, IDtoTypes
{
    public static Type CreateDTO => typeof(CreateUserDTO);
    public static Type UpdateDTO => typeof(UpdateUserDTO);
    public static Type BaseDTO => typeof(UserDTO);
    public static Type BaseType => typeof(Models.User);

    public string Name { get; set; }
    [ValueConverter(typeof(StringToEnumConverter<UserRole>))]
    public string Role { get; set; }
}

