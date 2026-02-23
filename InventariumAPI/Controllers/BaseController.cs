
using AutoMapper;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace InventariumAPI.Controllers;

public abstract class BaseController
    <TModel, TId, TDto, TCreateDTO, TUpdateDTO>
    (IBaseRepository<TModel, TId> _repository, IMapper _mapper)
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

        return entries.Take(1).ToDictionary(
            x => x.GetId(),
            x => _mapper.Map<TDto>(x)
        );
    }

    [HttpGet("{id}")]
    public async Task<FallibleResponse<TDto>> GetById(TId id)
        => _mapper.Map<TDto>(await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"The {typeof(TModel).Name} with the ID {id} doesn't exist."));

    [HttpPost]
    public async Task<FallibleResponse<TId>> Create(TCreateDTO data )
    {
        var mapped = _mapper.Map<TModel>(data);

        TModel entry = await _repository.CreateAsync(mapped);

        Console.WriteLine($"Created new entity with ID {entry.GetId()}");

        await _repository.SaveChangesAsync();


        return entry.GetId();
    }

    [HttpPut("{id}")]
    public async Task<FallibleResponse<TDto>> Update(TId id, [FromBody] TUpdateDTO update)
    {
        var entity = await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"The {typeof(TModel).Name} with the ID {id} doesn't exist.");

        _mapper.Map(update, entity);

        await _repository.UpdateAsync(entity);

        await _repository.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }

    [HttpDelete("{id}")]
    public async Task<FallibleResponse> Delete(TId id)
    {
        var entity = await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"The {typeof(TModel).Name} with the ID {id} doesn't exist.");

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();

        return new();
    }
}

