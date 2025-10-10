
#nullable disable
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using InventariumAPI.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventariumAPI.DTOs.User;
public class UserDTO : IBaseDTO<Models.User, int>, IDtoTypes
{
    public static Type CreateDTO => typeof(CreateUserDTO);
    public static Type UpdateDTO => typeof(UpdateUserDTO);
    public static Type BaseDTO => typeof(UserDTO);
    public static Type BaseType => typeof(Models.User);

    [ReferenceToModel(typeof(Models.User), typeof(int))]
    public int UserId { get; set; }

    public string Name { get; set; }
    [ValueConverter(typeof(StringToEnumConverter<UserRole>))]
    public string Role { get; set; } = nameof(UserRole.Customer);
}

