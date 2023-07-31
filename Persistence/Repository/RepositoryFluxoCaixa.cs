using Domain.Contract;
using Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;

namespace Persistence.Repository
{
  public class RepositoryFluxoCaixa : IRepositoryFluxoCaixa
  {
    private readonly FluxoCaixaContext _context;
    public RepositoryFluxoCaixa(IConfiguration configuration)
    {
      var options = new DbContextOptions<FluxoCaixaContext>();
      _context = new FluxoCaixaContext(configuration, options);
    }
    public async Task<Guid> AddBalance(Balance entidade, Record record)
    {
      record.IdAccountNavigation = _context.Accounts.Find(record.IdAccountNavigation.Id);
      entidade.IdAccountNavigation = _context.Accounts.Find(entidade.IdAccountNavigation.Id);
      _context.Add<Record>(record);
      var result = _context.Add<Balance>(entidade);
      await _context.SaveChangesAsync();
      return result.Entity.Id;
    }
    public async Task<int> UpdateBalance(Balance entidade, Record record)
    {
      record.IdAccountNavigation = _context.Accounts.Find(record.IdAccountNavigation.Id);
      entidade.IdAccountNavigation = _context.Accounts.Find(entidade.IdAccountNavigation.Id);
      _context.Add<Record>(record);
      var result = _context.Update<Balance>(entidade);
      return await _context.SaveChangesAsync();       
    }    
  }
}
