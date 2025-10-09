using InventariumAPI.Data;

namespace InventariumAPI.Interfaces;

public interface IDebugRepository
{
    public Task  Populate<TEntity>(int count)
                where TEntity : class, IFakerFactory<TEntity>;
    public Task WipeAllData<TEntity>()
        where TEntity : class;
}
