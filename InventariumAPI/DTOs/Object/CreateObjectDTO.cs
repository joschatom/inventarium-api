using AutoMapper.Configuration.Annotations;
using InventariumAPI.Data;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace InventariumAPI.DTOs.Object;

public class CreateObjectDTO: ObjectDTO
{


    public new required string Name { get; set; }
    public new required string Description { get; set; }
    public new required int LocationId{ get; set; }
    public new required int CategoryId { get; set; }

}


