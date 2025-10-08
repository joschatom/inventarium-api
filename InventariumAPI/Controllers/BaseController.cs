
using AutoMapper;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace InventariumAPI.Controllers;

public abstract class BaseController
    <TModel, TId, TDto, TCreateDTO, TUpdateDTO>
    (IBaseRepository<TModel, TId> _repository, IMapper _mapper, DataContext _context)
    : ControllerBase
    where TId : notnull
    where TModel : class, IGenericModel<TId>
    where TDto : class, IBaseDTO<TModel, TId>, IDtoTypes, new()
    where TCreateDTO : class, IBaseDTO<TModel, TId>
    where TUpdateDTO : class, IBaseDTO<TModel, TId>
{

    [HttpGet]
    public async Task<FallibleResponse<Dictionary<TId, TDto>>> GetAll()
    {
        TDto dto = new();

        var entries = await _repository.GetAllAsync();

        return new(entries.ToDictionary(
            x => x.GetId(),
            x => _mapper.Map<TDto>(x)
        ));
    }

    [HttpGet("{id}")]
    public async Task<FallibleResponse<TDto>> GetById(TId id)
        => new(_mapper.Map<TDto>(await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"The {typeof(TModel).Name} with the ID {id} doesn't exist.")));

    [HttpPost]
    public async Task<FallibleResponse<TId>> Create(TCreateDTO data )
    {
        var mapped = _mapper.Map<TModel>(data);

        TModel entry = await _repository.CreateAsync(mapped);

        Console.WriteLine($"Created new entity with ID {entry.GetId()}");

        await _repository.SaveChangesAsync();


        return new(entry.GetId());
    }

    [HttpPut("{id}")]
    public async Task<FallibleResponse<TDto>> Update(TId id, [FromBody] TUpdateDTO update)
    {
        var entity = await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"The {typeof(TModel).Name} with the ID {id} doesn't exist.");
        
        entity = _mapper.Map(update, entity);

        await _repository.UpdateAsync(entity);

        await _repository.SaveChangesAsync();

        return new(_mapper.Map<TDto>(entity));
    }
}

