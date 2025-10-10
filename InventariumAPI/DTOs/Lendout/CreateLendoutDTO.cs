using AutoMapper.Configuration.Annotations;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace InventariumAPI.DTOs.Lendout;

public class CreateLendoutDTO: IBaseDTO<Models.Lendout, (TModelId, TModelId)>
{
    [Ignore] private DateTime m_StartDate = default;
    [Ignore] private DateTime? m_EndDate = default;

    public required DateTime StartDate {
        get => m_StartDate;
        set => m_StartDate = value;
    }
    public DateTime? EndDate
    {
        get => m_EndDate;
        set => m_EndDate = value;
    }
}
