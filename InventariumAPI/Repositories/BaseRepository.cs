using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventariumAPI.Repositories;

public abstract class BaseRepository<TModel, TId>(DataContext context) : IBaseRepository<TModel, TId>
    where TModel : class, IGenericModel<TId>
    where TId : notnull
{
    private readonly DbSet<TModel> _dbSet = context.Set<TModel>();

    public async Task<TModel> CreateAsync(TModel model)
        => (await _dbSet.AddAsync(model)).Entity;

    public Task DeleteAsync(TId id)
        => Task.Run(async () => _dbSet.Remove(await GetAsync(id)
            ?? throw new KeyNotFoundException($"The {typeof(TModel).Name} with the ID {id} doesn't exist.")));

    public async Task<bool> DoesExistAsync(TId id)
        => (await _dbSet.FindAsync(id)) is not null;

    public async Task<IEnumerable<TModel>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<TModel?> GetAsync(TId id)
        => await _dbSet.FindAsync(id);

    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();

    public Task<TModel> UpdateAsync(TModel model)
        => Task.Run(() => _dbSet.Update(model).Entity);
}
