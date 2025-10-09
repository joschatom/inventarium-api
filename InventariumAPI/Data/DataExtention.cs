using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Reflection;

namespace InventariumAPI.Data;

public static class DataExtention
{
    public static IQueryable<TEntity> IncludeAll<TEntity>(
        this IQueryable<TEntity> source,
        IModel model,
        Func<INavigation, bool> predicate)
        where TEntity : class
    {
        var included = model.FindEntityType(typeof(TEntity))!
            .GetDeclaredNavigations()
            .Where(n => predicate(n));

        foreach (var include in included)
            source = source.Include(include.Name);

        return source;
    }
}
