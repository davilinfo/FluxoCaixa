using Application.Models.ViewModel;

namespace Application.Interfaces
{
  public interface IApplicationServiceBalance : IApplicationService<BalanceViewModel>
  {
    Task<BalanceViewModel> GetByAccountId(Guid accountId);
  }
}
