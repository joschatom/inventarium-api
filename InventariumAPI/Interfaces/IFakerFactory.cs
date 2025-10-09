using Bogus;
using InventariumAPI.Data;

namespace InventariumAPI.Interfaces;

public interface IFakerFactory<T>
    where T: class
{
    public abstract static Faker<T> CreateFaker(DataContext context);
}
