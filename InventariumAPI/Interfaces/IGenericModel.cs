using System.Security.Cryptography;

namespace InventariumAPI.Interfaces;
public interface IGenericModel
{
    public static Type GetIdType() => throw new NotImplementedException();
}

public interface IGenericModel<TId>: IGenericModel
{
    public new static Type GetIdType() => typeof(TId);

    public abstract TId GetId();
    public static virtual object[] DeconstructId(TId id) => [id!];
}