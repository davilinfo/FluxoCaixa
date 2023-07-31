using Application.Models.Request;
using Application.Models.ViewModel;

namespace Application.Interfaces
{
  public interface IApplicationServiceRecord : IApplicationService<RecordViewModel>
  {
    Task<RecordViewModel> AddAsync(RecordRequest recordRequest);
  }
}
