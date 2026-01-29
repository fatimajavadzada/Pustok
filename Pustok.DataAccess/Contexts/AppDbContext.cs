using Microsoft.EntityFrameworkCore;
using Pustok.Core.Entites;
using Pustok.Core.Entites.Common;
using Pustok.DataAccess.Interceptors;
using System.Reflection;

namespace Pustok.DataAccess.Contexts;

internal class AppDbContext(BaseAuditableInterceptor _interceptor, DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptor);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
