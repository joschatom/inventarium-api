using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;

namespace InventariumAPI.DTOs.Category;

public class UpdateCategoryDTO: CategoryDTO
{
    public new string? Name { get; set; }
}
