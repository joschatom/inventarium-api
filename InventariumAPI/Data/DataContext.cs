using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace InventariumAPI.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Models.ObjectEntry> Objects { get; set; }
}