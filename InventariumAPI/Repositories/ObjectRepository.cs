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

    public ValueTask<BrokenObject?> AsBrokenAsync(TModelId id)
        => context.BrokenObjects.FindAsync(id);

    public async Task<Lendout?> GetLendout(TModelId id)
        => await context.Lendouts
            .Where(l => l.ObjectId == id)
            .Where(l => l.StartDate <= DateTime.UtcNow 
                && (l.EndDate == null 
                || DateTime.UtcNow < (l.EndDate)
                )
            )
            .FirstOrDefaultAsync();
        
    public async Task<IEnumerable<User>?> GetManagers(TModelId id)
        => await context.ObjectManagers
            .Where(m => m.ObjectId == id)
            .ToListAsync()
            .ContinueWith(l => l.Result.Select(m => m.User));

    public async Task<bool?> SetBrokenAsync(int id, string? reason)
    {
        if (!await DoesExistAsync(id))
            return null;

        if (reason is not null)
        {
            var broken = await context.BrokenObjects.FindAsync(id);

            if (broken == null)
                return null;

            context.BrokenObjects.Remove(broken);

            return false;
        }

        context.BrokenObjects.Update(new BrokenObject()
        {
            ObjectId = id,
            Reason = reason!
        });

        return true;
    }
}
