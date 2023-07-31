using Application.Enum;
using Application.Exception;
using Application.Interfaces;
using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Contract;
using Domain.EF;
using System.Transactions;

namespace Application.Services
{
  public class RecordApplicationService : IApplicationServiceRecord
  {
    private readonly IMapper _mapper;    
    private readonly IRepositoryRecord _repositoryRecord;
    private readonly IRepositoryBalance _repositoryBalance;
    private readonly IRepositoryAccount _repositoryAccount;
    const string _contaInvalidaMsg = "Conta inválida";
    const string _contaSaldoInsuficienteMsg = "Saldo insuficiente";
    public RecordApplicationService(IMapper mapper, IRepositoryRecord repositoryRecord, IRepositoryBalance repositoryBalance, IRepositoryAccount repositoryAccount) 
    {
      _mapper = mapper;      
      _repositoryRecord = repositoryRecord;
      _repositoryBalance = repositoryBalance;
      _repositoryAccount = repositoryAccount;
    }

    public async Task<RecordViewModel> Add(RecordViewModel model)
    {
      model.DateTime = DateTime.UtcNow;
      var entity = _mapper.Map<Record>(model);

      await HandleValidation(entity);
      
      await AddRecordAndUpdateBalanceFromRecordInTransaction(entity);      

      model = _mapper.Map<RecordViewModel>(entity);

      return model;
    }

    public IEnumerable<RecordViewModel> GetByEmail(string email)
    {
      throw new NotImplementedException();
    }

    public Task<RecordViewModel> GetById(Guid id)
    {
      throw new NotImplementedException();
    }

    public Task<RecordViewModel> Update(RecordViewModel model)
    {
      throw new NotImplementedException();
    }

    private async Task HandleValidation(Record record)
    {
      if (record.IdAccountNavigation == null)
      {
        throw new BusinessException(_contaInvalidaMsg);
      }
      var account = await _repositoryAccount.GetById(record.IdAccountNavigation.Id);      
      if (account == null)
      {
        throw new BusinessException(_contaInvalidaMsg);
      }      

      var balance = _repositoryBalance.All().FirstOrDefault(b => b.IdAccountNavigation.Id == record.IdAccountNavigation.Id);
      
      if (balance == null && record.Type.ToString().ToLower() == ((char)RecordType.Debit).ToString().ToLower())
      {
        throw new BusinessException(_contaSaldoInsuficienteMsg);
      }
      if (balance != null && record.Type.ToString().ToLower() == ((char)RecordType.Debit).ToString().ToLower())
      {
        if (balance?.Value < record.Value)
          throw new BusinessException(_contaSaldoInsuficienteMsg);
      }
    }

    private async Task AddRecordAndUpdateBalanceFromRecordInTransaction(Record record)
    {      
      if (record != null)
      {

        await _repositoryRecord.Add(record);
        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {          
          var balance = _repositoryBalance.All().FirstOrDefault(b => b.IdAccountNavigation.Id == record.IdAccountNavigation.Id);
          if (balance == null)
          {
            var newBalance = new Balance()
            {
              Created = DateTime.UtcNow,
              IdAccountNavigation = record.IdAccountNavigation,
              Value = 0,
              Updated = DateTime.UtcNow
            };

            var guid = await _repositoryBalance.Add(newBalance);
            balance = await _repositoryBalance.GetById(guid);
          }

          if (record.Type.ToString().ToLower() == ((char)RecordType.Debit).ToString().ToLower())
          {
            if (balance.Value >= record.Value)
            {
              balance.Value -= record.Value;
            }
            else
            {
              throw new BusinessException(_contaSaldoInsuficienteMsg);
            }
          }

          if (record.Type.ToString().ToLower() == ((char)RecordType.Credit).ToString().ToLower())
          {
            balance.Value += record.Value;
          }

          if (await _repositoryBalance.Update(balance) >= 0)
          {
            scope.Complete();            
          }
          
        }
      }
    }

    public async Task<RecordViewModel> AddAsync(RecordRequest recordRequest)
    {
      var viewModel = _mapper.Map<RecordViewModel>(recordRequest);
      viewModel.History = recordRequest.Description;
      viewModel.IdAccountNavigation = new AccountViewModel { Id = (Guid.Parse(recordRequest.AccountId)) };
      
      var result = await Add(viewModel);

      return result;
    }
  }
}
