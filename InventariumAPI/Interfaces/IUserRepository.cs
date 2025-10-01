namespace InventariumAPI.Interfaces;

using InventariumAPI.Models;


public interface IUserRepository: IBaseRepository<Models.User, TModelId>
{
    public Task Promote(TModelId id);
    public Task Demote(TModelId id);

    public Task<IEnumerable<Lendout>> GetLendouts(TModelId id);
}
