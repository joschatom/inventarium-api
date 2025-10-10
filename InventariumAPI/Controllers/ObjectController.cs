using AutoMapper;
using InventariumAPI.Data;
using InventariumAPI.DTOs.Lendout;
using InventariumAPI.DTOs.Object;
using InventariumAPI.DTOs.User;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using InventariumAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.InteropServices;
using ObjectModel = InventariumAPI.Models.ObjectEntry;

namespace InventariumAPI.Controllers;

[Route("api/object")]
[ApiController]
public class ObjectController
    (IObjectRepository _repository, 
    IMapper _mapper,
    IUserRepository _userRepository)
    : BaseController<ObjectModel, TModelId,
        DTOs.Object.ObjectDTO,
        DTOs.Object.CreateObjectDTO,
        DTOs.Object.UpdateObjectDTO>
    (_repository, _mapper)
{
    private readonly IObjectRepository repository = _repository;
    private readonly IMapper mapper = _mapper;

    [HttpGet("{id}/managers")]
    public async Task<FallibleResponse<UserDTO[]>> GetManagers(TModelId id)
    {
        if (!await repository.DoesExistAsync(id))
            throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

        var managers = await repository.GetManagersOrDefault(id);

        return mapper.Map<UserDTO[]>(managers);
    }

    [HttpPost("{id}/managers")]
    public async Task<FallibleResponse> AddManager(TModelId id, TModelId userId)
    {
        var obj = await repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

        var user = await _userRepository.GetAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} doesn't exists.");

        if (user.Role < UserRole.Manager)
            throw new InvalidOperationException($"The {user} isn't a manager.");

        if (await repository.HasManagers(id, userId))
            throw new InvalidOperationException($"The {user} is already a manager for {obj}.");

        await repository.AddManager(id, userId);
        await repository.SaveChangesAsync();

        return new();
    }

    [HttpDelete("{id}/managers/{userId}")]
    public async Task<FallibleResponse> RemoveManager(TModelId id, TModelId userId)
    {
        var obj = await repository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

        var user = await _userRepository.GetAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} doesn't exists.");

        if (user.Role < UserRole.Manager)
            throw new InvalidOperationException($"The {user} isn't a manager.");

        if (!await repository.HasManagers(id, userId))
            throw new InvalidOperationException($"The {user} is not a manager for {obj}.");

        await repository.RemoveManager(id, userId);
        await repository.SaveChangesAsync();

        return new();
    }

    [HttpGet("{id}/lendout")]
    public async Task<FallibleResponse<LendoutDTO>> GetCurrentLendout(TModelId id)
        => mapper.Map<LendoutDTO>(
            await repository.GetLendout(id)
            ?? throw new KeyNotFoundException($"Lendout for object with ID {id} doesn't exists.")
        );

    [HttpGet("{id}/broken")]
    public async Task<FallibleResponse<BrokenObjectDTO>> GetAsBrokenObject(TModelId id)
    {
        if (!await repository.DoesExistAsync(id))
            throw new KeyNotFoundException($"Object with ID {id} doesn't exists.");

        var broken = await repository.AsBrokenAsync(id);

        return mapper.Map<BrokenObjectDTO?>(broken);
    }

    [HttpPatch("{id}/broken")]
    public async Task<FallibleResponse<bool>> SetBrokenObject(
        TModelId id,
        [FromBody] [MinLength(8)]
        string? reason
        )
    {
        var deleted = await repository.SetBrokenAsync(id, reason);

        _ = deleted ?? throw 
            new KeyNotFoundException($"Object with ID {id} doesn't exists.");

        await repository.SaveChangesAsync();

        return deleted;
    }
}
