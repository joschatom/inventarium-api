using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace InventariumAPI.Repositories;

public abstract class BaseRepository<TModel, TId> : IBaseRepository<TModel, TId>
    where TModel : class, IGenericModel<TId>
    where TId : notnull
{
    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<TModel>();
    }

    private readonly DbSet<TModel> _dbSet;
    private readonly DataContext _context;


    public async Task<TModel> CreateAsync(TModel model)
        => (await _dbSet.AddAsync(model)).Entity;

    public Task DeleteAsync(TModel model)
        => Task.Run(() => _dbSet.Remove(model));

    public async Task<bool> DoesExistAsync(TId id)
        => (await GetAsync(id)) is not null;

    public async Task<IEnumerable<TModel>> GetAllAsync()
        => await _dbSet
            .IncludeAll(_dbSet.EntityType.Model, n => !n.IsCollection)
            .ToListAsync();

    public virtual async Task<TModel?> GetAsync(TId id)
    {
        var entry = await _dbSet
            .FindAsync(id);

        return await _dbSet
            .Where(e => e == entry)
            .IncludeAll(_dbSet.EntityType.Model, n => !n.IsCollection)
            .SingleOrDefaultAsync();
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public Task<TModel> UpdateAsync(TModel model)
        => Task.Run(() => _dbSet.Update(model).Entity);
}
