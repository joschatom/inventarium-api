using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventariumAPI.Repositories;

public class LendoutsRepository(DataContext _context): 
    BaseRepository<Models.Lendout, (TModelId objectId, TModelId userId)>
    (_context), ILendoutsRepository
{
    public override async Task<Lendout?> GetAsync((int objectId, int userId) id)
    {
        await DeleteOldEntries();
        return await base.GetAsync(id);
    }
    
    public async Task DeleteOldEntries()
    {
        await _context
            .Lendouts
            .Where(l => l.EndDate < DateTime.UtcNow)
            .ExecuteDeleteAsync();

        await _context.SaveChangesAsync();
    }
}
