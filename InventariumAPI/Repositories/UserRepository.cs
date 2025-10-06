using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;

namespace InventariumAPI.Repositories;

public class UserRepository(DataContext _context)
    : BaseRepository<Models.User, TModelId>(_context)
    , IUserRepository
{
    public async Task<UserRole?> Demote(TModelId id)
    {
        var user = await base.GetAsync(id);
         
        if (user == null)
            return null;

        user.Role = user.Role switch
        {
            UserRole.Manager => UserRole.Customer,
            _ => throw new InvalidOperationException($"Cannot demote user with ID {id} and Role {user.Role}"),
        };
        await base.UpdateAsync(user);
        await base.SaveChangesAsync();

        return user.Role;
    }

    public async Task<IEnumerable<Lendout>?> GetLendouts(TModelId id)
    {
        var user = await base.GetAsync(id);

        if (user == null) 
            return null;
           
        return user.Lendouts.AsEnumerable();
    }

    public async Task<UserRole?> Promote(TModelId id)
    {
        var user = await base.GetAsync(id);

        if (user == null)
            return null;

        user.Role = user.Role switch
        {
            UserRole.Customer => UserRole.Manager,
            _ => throw new InvalidOperationException($"Cannot promote user with ID {id} and Role {user.Role}"),
        };
        await base.UpdateAsync(user);
        await base.SaveChangesAsync();

        return user.Role;
    }
}
