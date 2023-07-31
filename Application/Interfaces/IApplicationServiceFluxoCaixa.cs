using Application.Models.Request;
using Application.Models.ViewModel;

namespace Application.Interfaces
{
  public interface IApplicationServiceFluxoCaixa
  {
    Task<RecordViewModel> AddAsync(RecordRequest recordRequest);
  }
}
