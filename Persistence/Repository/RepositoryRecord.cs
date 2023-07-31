using Domain.Contract;
using Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Repository
{
  [ExcludeFromCodeCoverage]
  public class RepositoryRecord : IRepositoryRecord
  {
    private readonly FluxoCaixaContext _context;
    public RepositoryRecord(IConfiguration configuration)
    {
      var options = new DbContextOptions<FluxoCaixaContext>();
      _context = new FluxoCaixaContext(configuration, options);
    }
    public async Task<Guid> Add(Record entidade)
    {
      entidade.IdAccountNavigation = _context.Accounts.Find(entidade.IdAccountNavigation.Id);
      var result = _context.Add(entidade);
      await _context.SaveChangesAsync();
      return result.Entity.Id;
    }

    public IQueryable<Record> All()
    {
      return _context.Records.Include(b => b.IdAccountNavigation).AsNoTracking();
    }

    public Task<int> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public async Task<Record> GetById(Guid id)
    {
      var result = await _context.Records.Include(b => b.IdAccountNavigation).FirstOrDefaultAsync(t => t.Id == id);
      return result;
    }

    public async Task<int> Update(Record entidade)
    {
      throw new NotImplementedException();
    }
  }
}
