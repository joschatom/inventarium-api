using InventariumAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.AccessControl;

namespace InventariumAPI.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Models.ObjectEntry> Objects { get; set; }
    public DbSet<Models.Location> Locations { get; set; }
    public DbSet<Models.Category> Categories { get; set; }
    public DbSet<Models.User> Users { get; set; }
    public DbSet<Models.Lendout> Lendouts { get; set; }
    public DbSet<Models.ObjectManager> ObjectManagers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lendout>()
            .HasKey(k => new { k.ObjectId, k.UserId });
           
        modelBuilder.Entity<ObjectManager>()
            .HasKey(k => new { k.ObjectId, k.UserId });

        modelBuilder.Entity<ObjectEntry>()
            .HasOne<Location>(k => k.Location)
            .WithMany(k => k.Objects)
            .IsRequired();

        modelBuilder.Entity<ObjectEntry>()
            .HasOne(k => k.Category)
            .WithMany(k => k.Objects)
            .IsRequired();

        modelBuilder.Entity<ObjectEntry>()
            .HasMany(k => k.Managers)
            .WithOne(k => k.Object)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ObjectEntry>()
            .HasOne<Lendout>()
            .WithOne(l => l.Object)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(k => k.Lendouts)
            .WithOne(l => l.User)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}