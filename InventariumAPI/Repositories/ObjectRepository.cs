using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventariumAPI.Repositories;
public class ObjectRepository(DataContext _context)
    : BaseRepository<Models.ObjectEntry, TModelId>(_context)
    , IObjectRepository
{
    private readonly DataContext context = _context;

    public Task AddManager(TModelId id, TModelId userId)
        => Task.Run(() => context.ObjectManagers.AddAsync(new(){
            ObjectId = id,
            UserId = userId
        }));

    public ValueTask<BrokenObject?> AsBrokenAsync(TModelId id)
        => context.BrokenObjects.FindAsync(id);

    public async Task<Lendout?> GetLendout(TModelId id)
        => await context.Lendouts
            .IgnoreAutoIncludes()
            .Where(l => l.ObjectId == id)
            .Include(l => l.User)
            .Include(l => l.Object)
            .Where(l => l.StartDate < DateTime.UtcNow
                && (l.EndDate != null || (DateTime.UtcNow < (l.EndDate))
                )
            )
            .FirstOrDefaultAsync();
        
    public async Task<IEnumerable<User>> GetManagersOrDefault(TModelId id)
        => await context.ObjectManagers
            .IgnoreAutoIncludes()
            .Include(l => l.User)
            .Include(l => l.Object)
            .Where(m => m.ObjectId == id)
            .ToListAsync()
            .ContinueWith(l => l.Result.Select(m => m.User));

    public async Task<bool> HasManagers(TModelId id, TModelId userId)
    => await context.ObjectManagers
        .ContainsAsync(new() { UserId = userId, ObjectId = id });

    public async Task RemoveManager(int id, int userId)
        => await context.ObjectManagers
            .Where (m => m.ObjectId == id && m.UserId == userId)
            .ExecuteDeleteAsync();

    public async Task<bool?> SetBrokenAsync(int id, string? reason)
    {

        if (!await DoesExistAsync(id))
            return null;

        var broken = await context.BrokenObjects.FindAsync(id);

        if (broken is null && reason is null)
            return null;
        else if (reason is null && broken is not null)
        {
            context.BrokenObjects.Remove(broken);
            return true;
        } else if (reason is not null && broken is not null)
        {
            broken.Reason = reason!;
            context.Update(broken);
        } else if (reason is not null && broken is null)
        {
            await context.BrokenObjects.AddAsync(new BrokenObject()
            {
                ObjectId = id,
                Reason = reason!
            });
        }

        return false;
    }
}
