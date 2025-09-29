namespace InventariumAPI.Interfaces;

public interface IGenericModel<TId>
{
    public abstract TId GetId();
}