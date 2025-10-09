using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace InventariumAPI;

public class DebugControl
{
    public DebugControl(DataContext context, IDebugRepository repository)
    {
        var enities = context.Model.GetEntityTypes()
            .Select(e => e.ClrType);

        foreach (var entity_t in enities)
        {
            Console.WriteLine(entity_t.Name);

        
        }

    }
}
