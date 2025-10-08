using AutoMapper;
using InventariumAPI.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Interfaces;

public interface IBaseDTO<TModel, TId> where TModel : IGenericModel<TId>
{

    public IBaseDTO<TModel, TId> Combine<TOtherDto>(TOtherDto otherDto)
        where TOtherDto : IBaseDTO<TModel, TId>
        => otherDto;

}
