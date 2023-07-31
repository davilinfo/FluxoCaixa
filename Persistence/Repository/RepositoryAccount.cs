using Domain.Contract;
using Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Repository
{
  [ExcludeFromCodeCoverage]
  public class RepositoryAccount : IRepositoryAccount
  {
    private readonly FluxoCaixaContext _context;
    public RepositoryAccount(IConfiguration configuration) {
      var options = new DbContextOptions<FluxoCaixaContext>();
      _context = new FluxoCaixaContext(configuration, options);    
    }
    public async Task<Guid> Add(Account entidade)
    {
      var result = _context.Add(entidade);
      await _context.SaveChangesAsync();
      return result.Entity.Id;
    }

    public IQueryable<Account> All()
    {
      return _context.Accounts.AsNoTracking();
    }

    public async Task<int> Delete(Guid id)
    {
      var entity = _context.Accounts.Find(id);
      if (entity != null)
      {
        _context.Accounts.Remove(entity);
      }
      return await _context.SaveChangesAsync();
    }

    public async Task<Account> GetById(Guid id)
    {
      var result = await _context.Accounts.FirstOrDefaultAsync(t => t.Id == id);
      return result;
    }

    public async Task<int> Update(Account entidade)
    {
      _context.Accounts.Update(entidade);

      return await _context.SaveChangesAsync();
    }
  }
}
