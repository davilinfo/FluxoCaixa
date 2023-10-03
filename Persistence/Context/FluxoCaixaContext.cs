using Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Context
{
  [ExcludeFromCodeCoverage]
  public class FluxoCaixaContext : DbContext
  {
    IConfiguration _configuration;
    const string _connectionString = "DefaultConnection";

    public DbSet<Account> Accounts { get; private set; }
    public DbSet<Balance> Balances { get; private set; }
    public DbSet<Extract> Extracts { get; private set; }
    public DbSet<Record> Records { get; private set; }

    public FluxoCaixaContext(IConfiguration configuration, DbContextOptions<FluxoCaixaContext> options): base(options)
    {
      _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_configuration.GetConnectionString(_connectionString), o => o.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Account>().Property<DateTime>(p => p.Created).HasDefaultValue(DateTime.UtcNow);
      modelBuilder.Entity<Account>().Property<DateTime?>(p => p.Updated).HasDefaultValue(DateTime.UtcNow);      
      modelBuilder.Entity<Balance>().Property<DateTime>(p=> p.Created).HasDefaultValue(DateTime.UtcNow);
      modelBuilder.Entity<Balance>().Property<DateTime>(p => p.Updated).HasDefaultValue(DateTime.UtcNow);
      modelBuilder.Entity<Extract>().Property<DateTime>(p => p.Created).HasDefaultValue(DateTime.UtcNow);
      modelBuilder.Entity<Record>().Property<DateTime>(p => p.DateTime).HasDefaultValue(DateTime.UtcNow);      
    }
  }
}
