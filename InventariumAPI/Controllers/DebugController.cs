using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using InventariumAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Storage;
using MoreLinq;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventariumAPI.Controllers;

[ApiController]
[Route("/api/debug")]
public class DebugController(IDebugRepository repository,
    IUserRepository userRepository,
    IObjectRepository objectRepository,
    ILendoutsRepository lendoutsRepository,
    DataContext context,
    IHostEnvironment environment) 
    : ControllerBase
{
    private readonly Random rng = new Random();

    [HttpGet("wipeall")]
    public async Task<IActionResult> WipeAll()
    {
        if (!environment.IsDevelopment())
            return Forbid();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Requested wipeout, confirm with 'wipeall' to delete EVERYTHING!!");
        Console.ResetColor();
        if (Console.ReadLine() != "wipeall")
            return Unauthorized("Denied by server.");

        await repository.WipeAllData<ObjectManager>();
        await repository.WipeAllData<BrokenObject>();
        await repository.WipeAllData<Lendout>();
        await repository.WipeAllData<User>();
        await repository.WipeAllData<ObjectEntry>();
        await repository.WipeAllData<Location>();
        await repository.WipeAllData<Category>();
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("populate")]
    public async Task<ActionResult<FallibleResponse>> Populate(
        [FromQuery] int userCount = 1,
        [FromQuery] int locationCount = 1,
        [FromQuery] int categoryCount = 1,
        [FromQuery] int objectCount = 1,
        [FromQuery] int maxManagersPerObject = 3,
        [FromQuery][Range(0, 100)] int brokenProp = 20,
        [FromQuery][Range(0, 100)] int lendoutProp = 40,
        [FromQuery][Range(0, 100)] int managerProp = 65,
        [FromQuery][Range(0, 100)] int indefiniteLendoutProp = 2,
        [FromQuery][Range(0.0, 1.0)] double brokenLendoutMult = 0.25
    )
    {
        if (!environment.IsDevelopment())
            return Forbid();

        

        StringBuilder log = new StringBuilder();

        await repository.Populate<Location>(locationCount);
        await repository.Populate<Category>(categoryCount);
        await repository.Populate<User>(userCount);

        log.Append(context.ChangeTracker.DebugView.LongView);
        await userRepository.SaveChangesAsync();

        var users = await userRepository.GetAllAsync();

        var wordgen = new RandomDataGenerator.Randomizers.RandomizerTextWords(new()
        {
            Min = 8,
            Max = 125,
        });

        var dategen = new RandomDataGenerator.Randomizers.RandomizerDateTime(new()
        {
           IncludeTime = false,
           From = DateTime.UtcNow,
        });

        Console.WriteLine(wordgen.Generate());

        foreach (var u in users)
        {
            if (u.Role != UserRole.Customer)
                continue;

            if (rng.Next(0, 100) <= managerProp)
            {
                u.Role = UserRole.Manager;
            }
        }

        log.Append(context.ChangeTracker.DebugView.LongView);
        await userRepository.SaveChangesAsync();
        

        _ = 0;

        for (int i = 0; i < objectCount; i++)
        {
            var o = (await context.AddAsync(ObjectEntry
                .CreateFaker(context)
                .Generate()
            )).Entity;

            var managerCount = rng.Next(1, maxManagersPerObject);

            var managers = users
                .Where(u => u.Role == UserRole.Manager)
                .Select(u => u.UserId);

            if (managerCount > managers.Count())
            {
                Console.WriteLine(
                    "WARNING: Not enought valid user to assign as manager for" 
                    + $"{o}! TRUNCATING TO {managers.Count()}");
                managerCount = managers.Count();
            }

            await context.ObjectManagers.AddRangeAsync(
                managers.RandomSubset(managerCount)
                    .Select(uid => new ObjectManager() { 
                        Object = o, 
                        UserId = uid 
                    })
            );

            log.Append(context.ChangeTracker.DebugView.LongView);
            await userRepository.SaveChangesAsync();

            var broken = rng.Next(0, 100) <= brokenProp;
            if (broken)
                await objectRepository.SetBrokenAsync(o.ObjectId, wordgen.Generate());
            

            if (rng.Next(0, 100) <= (lendoutProp * (broken ? brokenLendoutMult : 1))) {
                var lender = users.RandomSubset(1).SingleOrDefault();
                var indefinitly = rng.Next(0, 100) <= indefiniteLendoutProp;

                if (lender != null)
                    await lendoutsRepository.CreateAsync(new() {
                        ObjectId = o.ObjectId,
                        UserId = lender.UserId,
                        StartDate = DateTime.Now,
                        EndDate = indefinitly ? null : dategen.Generate()!
                    });
            }

        }

        log.Append(context.ChangeTracker.DebugView.LongView);
        await userRepository.SaveChangesAsync();

        return Ok(new());
    }
}