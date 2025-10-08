using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.DTOs.Category;

public class UpdateCategoryDTO: CategoryDTO
{
    [MinLength(1)]
    public new string? Name { get; set; }
}
