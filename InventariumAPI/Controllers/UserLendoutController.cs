using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InventariumAPI.DTOs.Lendout;
using InventariumAPI.Interfaces;
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
    public async Task<IEnumerable<LendoutDTO>> GetAllLendouts([FromRoute] TModelId userId)
        => mapper.Map<IEnumerable<LendoutDTO>>((await repository.GetAllAsync())
            .Where(l => l.UserId == userId)
            .ToList());

    [HttpGet("{objectId}")]
    public async Task<LendoutDTO> GetLendout(TModelId userId, TModelId objectId)
    {
        Console.WriteLine(objectId);
        Console.WriteLine(userId);

        var lendout = await repository.GetAsync((objectId, userId))
            ?? throw new 
                KeyNotFoundException($"Lendout for object with ID {objectId} by user with ID {userId} doesn't exist.");

        return mapper.Map<LendoutDTO>(lendout);
    }

    [HttpPost("{objectId}")]
    public async Task<LendoutDTO> CreateLendout(
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
                InvalidOperationException($"The object '{obj?.Name}'(ID {obj?.ObjectId}) "
                + $"is already lent out to user {lender.Name}(ID {lender.UserId}) "
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
        if (!await userRepository.DoesExistAsync(userId))
            throw new KeyNotFoundException($"User with ID {userId} doesn't exist.");

        if (!await objectRepository.DoesExistAsync(objectId))
            throw new KeyNotFoundException($"Object with ID {objectId} doesn't exist.");

        await repository.DeleteAsync((objectId, userId));
        await repository.SaveChangesAsync();
    }
}
