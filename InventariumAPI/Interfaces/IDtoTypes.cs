namespace InventariumAPI.Interfaces;

public interface IDtoTypes
{
    public abstract static Type CreateDTO { get; }
    public abstract static Type UpdateDTO { get; }
    public abstract static Type BaseDTO { get; }
    
    public abstract static Type BaseType { get; }
}
