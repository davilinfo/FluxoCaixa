using Application.Enum;
using Application.Exception;
using Application.Interfaces;
using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Contract;
using Domain.EF;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace Application.Services
{
  [ExcludeFromCodeCoverage]
  public class RecordApplicationService : IApplicationServiceRecord
  {
    private readonly IMapper _mapper;    
    private readonly IRepositoryRecord _repositoryRecord;
    private readonly IRepositoryBalance _repositoryBalance;
    private readonly IRepositoryAccount _repositoryAccount;
    private readonly IRepositoryFluxoCaixa _repositoryFluxoCaixa;
    const string _contaInvalidaMsg = "Conta inválida";
    const string _contaSaldoInsuficienteMsg = "Saldo insuficiente";
    public RecordApplicationService(
      IMapper mapper, 
      IRepositoryRecord repositoryRecord, 
      IRepositoryBalance repositoryBalance, 
      IRepositoryAccount repositoryAccount,
      IRepositoryFluxoCaixa repositoryFluxoCaixa) 
    {
      _mapper = mapper;      
      _repositoryRecord = repositoryRecord;
      _repositoryBalance = repositoryBalance;
      _repositoryAccount = repositoryAccount;
      _repositoryFluxoCaixa = repositoryFluxoCaixa;
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
        var balance = _repositoryBalance.All().FirstOrDefault(b => b.IdAccountNavigation.Id == record.IdAccountNavigation.Id);
        if (balance == null)
        {
          if (record.Type.ToString().ToLower() == ((char)RecordType.Debit).ToString().ToLower())
          {            
            throw new BusinessException(_contaSaldoInsuficienteMsg);            
          }
          var newBalance = new Balance()
          {
            Created = DateTime.UtcNow,
            IdAccountNavigation = record.IdAccountNavigation,
            Value = record.Value,
            Updated = DateTime.UtcNow
          };

          var guid = await _repositoryFluxoCaixa.AddBalance(newBalance, record);
        }
        else
        {
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

          await _repositoryFluxoCaixa.UpdateBalance(balance, record);
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
