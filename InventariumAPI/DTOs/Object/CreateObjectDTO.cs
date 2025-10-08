using AutoMapper.Configuration.Annotations;
using InventariumAPI.Data;
using InventariumAPI.Models;
using InventariumAPI.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace InventariumAPI.DTOs.Object;

public class CreateObjectDTO: ObjectDTO
{
    [DefaultValue("free")]
    [AllowedValues(
        ["free", "broken"],
        ErrorMessage = "Creating an object with a state other than free or broken is not allowed.")]
    public new string State { get; set; } = "free";

    [DeniedValues([null])]
    [MinLength(3)]
    public new required string Name { get; set; }
    public new required string Description { get; set; }

    [CustomValidation(typeof(ValidationHelpers), nameof(ValidationHelpers.ExistsInDatabase))]
    public new required int LocationId { get; set; }

    [CustomValidation(typeof(ValidationHelpers), nameof(ValidationHelpers.ExistsInDatabase))]
    public new required int CategoryId { get; set; }

}


