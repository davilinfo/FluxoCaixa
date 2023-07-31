using Domain.Contract;
using Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Repository
{
  [ExcludeFromCodeCoverage]
  public class RepositoryBalance : IRepositoryBalance
  {
    private readonly FluxoCaixaContext _context;
    public RepositoryBalance(IConfiguration configuration)
    {
      var options = new DbContextOptions<FluxoCaixaContext>();
      _context = new FluxoCaixaContext(configuration, options);
    }
    public async Task<Guid> Add(Balance entidade)
    {
      entidade.IdAccountNavigation = _context.Accounts.Find(entidade.IdAccountNavigation.Id);
      var result = _context.Add(entidade);
      await _context.SaveChangesAsync();
      return result.Entity.Id;
    }

    public IQueryable<Balance> All()
    {
      return _context.Balances.Include(b => b.IdAccountNavigation).AsNoTracking();
    }

    public Task<int> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public async Task<Balance> GetById(Guid id)
    {
      var result = await _context.Balances.Include(b=> b.IdAccountNavigation).FirstOrDefaultAsync(t => t.Id == id);
      return result;
    }

    public async Task<int> Update(Balance entidade)
    {      
      _context.Balances.Update(entidade);

      return await _context.SaveChangesAsync();
    }
  }
}
