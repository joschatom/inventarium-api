namespace InventariumAPI.Interfaces
{
    public interface IBaseRepository<TModel, TId> where TModel : class
    {
        public Task<bool> DoesExistAsync(TId id);
        public Task<IEnumerable<TModel>> GetAllAsync();
        public Task<TModel?> GetAsync(TId id);
        public Task<TModel> CreateAsync(TModel model);
        public Task<TModel> UpdateAsync(TModel model);
        public Task DeleteAsync(TId id);

        public Task SaveChangesAsync();
    }
}
