using Application.Enum;
using Application.Exception;
using Application.Interfaces;
using Domain.Contract;
using Domain.EF;
using System.Transactions;

namespace Application.Services
{
  public class FluxoCaixaApplicationService : IApplicationServiceFluxoCaixa
  {    
    private readonly IRepositoryRecord _repositoryRecord;
    private readonly IRepositoryBalance _repositoryBalance;
    const string _contaSemSaldoSuficiente = "Conta sem saldo suficiente";

    public FluxoCaixaApplicationService(IRepositoryRecord repositoryRecord, IRepositoryBalance repositoryBalance)
    {
      _repositoryBalance = repositoryBalance;      
      _repositoryRecord = repositoryRecord;
    }
    public async Task UpdateBalanceFromRecord(Guid recordId)
    {
      var record = await _repositoryRecord.GetById(recordId);

      if (record != null) {
        var balance = _repositoryBalance.All().FirstOrDefault(b => b.IdAccountNavigation.Id == record.IdAccountNavigation.Id);

        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
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

          if (record.Type == (char)RecordType.Debit)
          {
            if (balance.Value >= record.Value)
            {
              balance.Value -= record.Value;
            }
            else
            {
              throw new BusinessException(_contaSemSaldoSuficiente);
            }
          }

          if (record.Type == (char)RecordType.Credit)
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
  }
}
