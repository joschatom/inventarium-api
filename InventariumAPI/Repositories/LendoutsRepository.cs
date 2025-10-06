using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Models;

namespace InventariumAPI.Repositories;

public class LendoutsRepository(DataContext _context): 
    BaseRepository<Models.Lendout, (TModelId objectId, TModelId userId)>
    (_context), ILendoutsRepository
{

}
