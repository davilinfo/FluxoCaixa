using Application.Models.Request;
using Application.Models.ViewModel;

namespace Application.Interfaces
{
  public interface IApplicationServiceAccount : IApplicationService<AccountViewModel>
  {
    Task<AccountViewModel> AddAsync(AccountRequest request);
  }
}
