namespace InventariumAPI.Interfaces;

public interface IObjectRepository: IBaseRepository<Models.ObjectEntry, TModelId>
{
    public Task<IEnumerable<Models.User>?> GetManagers(TModelId id);
    public Task<Models.Lendout?> GetLendout(TModelId id);

}
