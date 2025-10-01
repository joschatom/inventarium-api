using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;

namespace InventariumAPI.Repositories;

public class UserRepository(DataContext _context)
    : BaseRepository<Models.User, TModelId>(_context)
    , IUserRepository
{
    public async Task Demote(TModelId id)
    {
        var user = await base.GetAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} doesn' exists.");

        user.Role = user.Role switch
        {
            UserRole.Manager => UserRole.Customer,
            _ => throw new InvalidOperationException($"Cannot demote user with ID {id} and Role {user.Role}"),
        };
        await base.UpdateAsync(user);
        await base.SaveChangesAsync();
    }

    public async Task<IEnumerable<Lendout>> GetLendouts(TModelId id)
    {
        var user = await base.GetAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} doesn' exists.");

        return user.Lendouts.AsEnumerable();
    }

    public async Task Promote(TModelId id)
    {
        var user = await base.GetAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} doesn' exists.");

        user.Role = user.Role switch
        {
            UserRole.Customer => UserRole.Manager,
            _ => throw new InvalidOperationException($"Cannot promote user with ID {id} and Role {user.Role}"),
        };
        await base.UpdateAsync(user);
        await base.SaveChangesAsync();
    }
}
