using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using InventariumAPI.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventariumAPI.DTOs.Object;

public class UpdateObjectDTO: ObjectDTO
{
    [SwaggerIgnore]
    [AllowedValues([null], ErrorMessage = "Cannot change state of object directly.")]
    public new string? State { get; set; } = null;

    [MinLength(1)]
    public new string? Name { get; set; } = null;
    public new string? Description { get; set; } = null;

    [CustomValidation(typeof(ValidationHelpers), nameof(ValidationHelpers.ExistsInDatabase))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public new int? LocationId { get; set; } = null;

    [CustomValidation(typeof(ValidationHelpers), nameof(ValidationHelpers.ExistsInDatabase))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public new int? CategoryId { get; set; } = null;
}
