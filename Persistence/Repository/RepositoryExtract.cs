using Domain.Contract;
using Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Repository
{
  [ExcludeFromCodeCoverage]
  public class RepositoryExtract : IRepositoryExtract
  {
    private readonly FluxoCaixaContext _context;
    public RepositoryExtract(IConfiguration configuration)
    {
      var options = new DbContextOptions<FluxoCaixaContext>();
      _context = new FluxoCaixaContext(configuration, options);
    }
    public async Task<Guid> Add(Extract entidade)
    {
      var result = _context.Add(entidade);
      await _context.SaveChangesAsync();
      return result.Entity.Id;
    }

    public IQueryable<Extract> All()
    {
      return _context.Extracts.Include(e=> e.IdAccountNavigation).Include(r=>r.IdRecordNavigation).Include(ra=> ra.IdRecordNavigation.IdAccountNavigation).AsNoTracking();
    }

    public Task<int> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public async Task<Extract> GetById(Guid id)
    {
      var result = await _context.Extracts.Include(e => e.IdAccountNavigation).Include(r => r.IdRecordNavigation).Include(ra => ra.IdRecordNavigation.IdAccountNavigation).FirstOrDefaultAsync(t => t.Id == id);
      return result;
    }

    public async Task<int> Update(Extract entidade)
    {
      _context.Extracts.Update(entidade);

      return await _context.SaveChangesAsync();
    }
  }
}
