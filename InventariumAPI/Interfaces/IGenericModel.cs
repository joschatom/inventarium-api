namespace InventariumAPI.Interfaces;

public interface IGenericModel<TId>
{
    public abstract TId GetId();
    public static virtual object[] DeconstructId(TId id) => [id!];
}