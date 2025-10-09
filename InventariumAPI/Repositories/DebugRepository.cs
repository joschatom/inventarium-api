using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventariumAPI.Repositories;

public class DebugRepository(DataContext context) : IDebugRepository
{
    public async Task Populate<TEntity>(int count)
        where TEntity : class, IFakerFactory<TEntity>
    {
        var faker = TEntity.CreateFaker(context);

        await context.AddRangeAsync(faker.Generate(count));
        await context.SaveChangesAsync();
    }

    public Task WipeAllData<TEntity>()
        where TEntity: class
        => context.Set<TEntity>()
            .ExecuteDeleteAsync();
}
