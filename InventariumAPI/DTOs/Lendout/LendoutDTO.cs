
#nullable disable
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.DTOs.Location;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace InventariumAPI.DTOs.Lendout;
public class LendoutDTO : IBaseDTO<Models.Lendout, (TModelId, TModelId)>, IDtoTypes
{
    public static Type CreateDTO => typeof(CreateLendoutDTO);
    public static Type UpdateDTO => new{}.GetType();
    public static Type BaseDTO => typeof(LendoutDTO);
    public static Type BaseType => typeof(Models.Lendout);

    public TModelId UserId { get; set; }
    public TModelId ObjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }


    public static ValidationResult ValidateCreate(CreateLendoutDTO dto)
    {
   

        return ValidationResult.Success;
    }
}

static class DateTimeExt
{
    public static DateTime AlignToMinutes(this DateTime date)
        => date.Date + TimeSpan.FromSeconds(Math.Floor(date.TimeOfDay.TotalMinutes));
}

