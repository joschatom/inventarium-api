using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.DTOs.Lendout;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Runtime.Remoting;

namespace InventariumAPI.Controllers;



[Route("api/user/{userId}/lendouts")]
[ApiController]
public class UserLendoutController
    (ILendoutsRepository repository,
    IObjectRepository objectRepository,
    IUserRepository userRepository,
    IMapper mapper)
    : ControllerBase
{

    [HttpGet]
    public async Task<FallibleResponse<IEnumerable<LendoutDTO>>> GetAllLendouts([FromRoute] TModelId userId)
        => new(mapper.Map<IEnumerable<LendoutDTO>>((await repository.GetAllAsync())
            .Where(l => l.UserId == userId)
            .ToList()));

    [HttpGet("{objectId}")]
    public async Task<FallibleResponse<LendoutDTO>> GetLendout(TModelId userId, TModelId objectId)
    {
        var user = await userRepository.GetAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} doesn't exist.");

        var obj = await objectRepository.GetAsync(objectId)
            ?? throw new KeyNotFoundException($"Object with ID {objectId} doesn't exist.");


        Console.WriteLine(objectId);
        Console.WriteLine(userId);

        var lendout = await repository.GetAsync((objectId, userId))
            ?? throw new 
                KeyNotFoundException($"Lendout for {obj} by {user} doesn't exist.");

        return mapper.Map<LendoutDTO>(lendout);
    }

    [HttpPost("{objectId}")]
    public async Task<FallibleResponse<LendoutDTO>> CreateLendout(
        TModelId userId,
        TModelId objectId,
        [FromForm] TimeSpan? duration)
    {

        if (!await userRepository.DoesExistAsync(userId))
            throw new KeyNotFoundException($"User with ID {userId} doesn't exist.");

        var obj = await objectRepository.GetAsync(objectId)
            ?? throw new KeyNotFoundException($"Object with ID {userId} doesn't exist.");

        var lendout = await objectRepository.GetLendout(objectId);

        var startDate = DateTime.UtcNow.AlignToMinutes();
        var endDate = duration is not null
            ? startDate + duration : null;

        if (lendout != null)
        {
            var lender = await userRepository.GetAsync(lendout.UserId)
                ?? throw new Exception("Lender is not a real user.");

            throw new
                InvalidOperationException($"The {obj} "
                + $"is already lent out to {lender} "
                + (lendout.EndDate.HasValue
                    ? $"until {lendout.EndDate?.ToShortDateString()}."
                    : "indefinitly."));
        }

        await repository.DeleteOldEntries();

        lendout = new Models.Lendout()
        {
            UserId = userId,
            ObjectId = objectId,
            StartDate = startDate,
            EndDate = endDate,
        };

        lendout.ObjectId = objectId;
        lendout.UserId = userId;

        lendout = await repository.CreateAsync(lendout);

        await repository.SaveChangesAsync();

        return mapper.Map<LendoutDTO>(lendout);
    }

    [HttpDelete("{objectId}")]
    public async Task ReturnObject(TModelId userId, TModelId objectId)
    {
        var user = await userRepository.GetAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} doesn't exist.");

        var obj = await objectRepository.GetAsync(objectId)
            ?? throw new KeyNotFoundException($"Object with ID {objectId} doesn't exist.");

        var lendout = await repository.GetAsync((objectId, userId))
            ?? throw new InvalidOperationException(
                $"The {obj} is not lent out to {user}."
            );


        await repository.DeleteAsync(lendout);
        await repository.SaveChangesAsync();
    }
}
