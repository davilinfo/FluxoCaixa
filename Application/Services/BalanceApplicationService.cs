using Application.Exception;
using Application.Interfaces;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Contract;
using Domain.EF;

namespace Application.Services
{
  public class BalanceApplicationService : IApplicationServiceBalance
  {
    private readonly IMapper _mapper;
    private readonly IRepositoryAccount _repositoryAccount;
    private readonly IRepositoryBalance _repositoryBalance;
    const string _errorSaldoMsg = "Falha ao atualizar o saldo";

    public BalanceApplicationService(IMapper mapper, IRepositoryAccount repositoryAccount, IRepositoryBalance repositoryBalance)
    {
      _mapper = mapper;
      _repositoryAccount = repositoryAccount;
      _repositoryBalance = repositoryBalance;
    } 

    public async Task<BalanceViewModel> Add(BalanceViewModel model)
    {
      model.Created = DateTime.UtcNow;
      model.Updated = DateTime.UtcNow;

      var entity = _mapper.Map<Balance>(model);
      entity.IdAccountNavigation = _mapper.Map<Account>(model.IdAccountNavigation);
      var guid = await _repositoryBalance.Add(entity);

      var result = _mapper.Map<BalanceViewModel>(entity);

      return result;
    }
    
    public IEnumerable<BalanceViewModel> GetByEmail(string email)
    {
      throw new NotImplementedException();
    }

    public async Task<BalanceViewModel> GetByAccountId(Guid id)
    {
      var account = await _repositoryAccount.GetById(id);

      if (account != null)
      {
        var balance = _repositoryBalance.All().FirstOrDefault(b => b.IdAccountNavigation != null && b.IdAccountNavigation.Id == id);
        if (balance != null)
        {
          var result = _mapper.Map<BalanceViewModel>(balance);
          return result;
        }        
      }

      return null;
    }

    public async Task<BalanceViewModel> GetById(Guid id)
    {
      var balance = await _repositoryBalance.GetById(id);
      if (balance != null)
      {
        var result = _mapper.Map<BalanceViewModel>(balance);
        return result;
      }

      return null;
    }

    public async Task<BalanceViewModel> Update(BalanceViewModel model)
    {
      var balance = await _repositoryBalance.GetById(model.Id);
      if (balance != null)
      {
        balance.Updated = DateTime.UtcNow;
        balance.Value = model.Value;
        var updated = await _repositoryBalance.Update(balance);
        if (updated <= 0)
        {
          throw new BusinessException(_errorSaldoMsg);
        }

        return _mapper.Map<BalanceViewModel>(balance);
      }

      return null;
    }    
  }
}
