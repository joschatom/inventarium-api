namespace InventariumAPI.Interfaces;

using InventariumAPI.Models;


public interface IUserRepository: IBaseRepository<Models.User, TModelId>
{
    public Task<UserRole?> Promote(TModelId id);
    public Task<UserRole?> Demote(TModelId id);
}
