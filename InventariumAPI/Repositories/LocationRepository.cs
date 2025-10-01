using InventariumAPI.Data;
using InventariumAPI.Interfaces;

namespace InventariumAPI.Repositories;

public class LocationRepository(DataContext _context)
    : BaseRepository<Models.Location, int>(_context)
    , ILocationRepository;
