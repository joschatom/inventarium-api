using InventariumAPI.Data;
using InventariumAPI.Interfaces;

namespace InventariumAPI.Repositories;

public class CategoryRepository(DataContext _context)
    : BaseRepository<Models.Category, int>(_context)
    , ICategoryRepository;
