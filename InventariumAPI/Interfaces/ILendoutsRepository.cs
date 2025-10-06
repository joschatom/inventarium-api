namespace InventariumAPI.Interfaces;

using InventariumAPI.Models;


public interface ILendoutsRepository: IBaseRepository<Models.Lendout, (TModelId objectId, TModelId userId)>
{
}
