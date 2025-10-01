
#nullable disable
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Category;
public class CategoryDTO : IBaseDTO<Models.Category, int>, IDtoTypes
{
    public static Type CreateDTO => typeof(CreateCategoryDTO);
    public static Type UpdateDTO => typeof(UpdateCategoryDTO);
    public static Type BaseDTO => typeof(CategoryDTO);
    public static Type BaseType => typeof(Models.Category);

    public string Name { get; set; }
}

