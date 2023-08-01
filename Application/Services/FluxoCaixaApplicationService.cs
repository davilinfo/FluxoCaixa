using Application.Enum;
using Application.Exception;
using Application.Interfaces;
using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Contract;
using Domain.EF;
using System.Diagnostics.CodeAnalysis;

namespace Application.Services
{
  [ExcludeFromCodeCoverage]
  public class FluxoCaixaApplicationService : IApplicationServiceFluxoCaixa
  {
    private readonly IMapper _mapper;
    private readonly IRepositoryFluxoCaixa _repositoryFluxoCaixa;
    private readonly IRepositoryBalance _repositoryBalance;
    private readonly IRepositoryAccount _repositoryAccount;
    private readonly IRepositoryExtract _repositoryExtract;
    const string _contaInvalidaMsg = "Conta inválida";
    const string _contaSaldoInsuficienteMsg = "Saldo insuficiente";    

    public FluxoCaixaApplicationService(IMapper mapper, 
      IRepositoryAccount repositoryAccount, 
      IRepositoryFluxoCaixa repositoryFluxoCaixa, 
      IRepositoryBalance repositoryBalance,
      IRepositoryExtract repositoryExtract)
    {
      _mapper = mapper;
      _repositoryBalance = repositoryBalance;
      _repositoryFluxoCaixa = repositoryFluxoCaixa;
      _repositoryAccount = repositoryAccount;
      _repositoryExtract = repositoryExtract;
    }
    public async Task<RecordViewModel> AddAsync(RecordRequest recordRequest)
    {
      var viewModel = _mapper.Map<RecordViewModel>(recordRequest);
      viewModel.History = recordRequest.Description;
      viewModel.IdAccountNavigation = new AccountViewModel { Id = (Guid.Parse(recordRequest.AccountId)) };

      var result = await Add(viewModel);

      return result;
    }
    private async Task<RecordViewModel> Add(RecordViewModel model)
    {
      model.DateTime = DateTime.UtcNow;
      var entity = _mapper.Map<Record>(model);

      await HandleValidation(entity);

      await AddRecordUpdateBalanceFromRecord(entity);

      model = _mapper.Map<RecordViewModel>(entity);

      return model;
    }

    private async Task AddRecordUpdateBalanceFromRecord(Record record)
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

          var extract = new Extract() { 
            Value = newBalance.Value, 
            Created = DateTime.UtcNow, 
            IdRecordNavigation = record, 
            IdAccountNavigation = record.IdAccountNavigation 
          };

          var guid = await _repositoryFluxoCaixa.AddBalance(newBalance, record, extract);
        }
        else
        {
          balance.Updated = DateTime.UtcNow;
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

          var extract = new Extract()
          {
            Value = balance.Value,
            Created = DateTime.UtcNow,
            IdRecordNavigation = record,
            IdAccountNavigation = record.IdAccountNavigation
          };

          await _repositoryFluxoCaixa.UpdateBalance(balance, record, extract);
        }
      }
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
  }
}
