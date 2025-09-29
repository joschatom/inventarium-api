using InventariumAPI.Data;
using InventariumAPI.Interfaces;

namespace InventariumAPI.Repositories;

public class ObjectRepository(DataContext _context)
    : BaseRepository<Models.ObjectEntry, int>(_context)
    , IObjectRepository;
