using Domain.EF;

namespace Domain.Contract
{
  public interface IRepositoryFluxoCaixa
  {
    Task<Guid> AddBalance(Balance entidade, EF.Record record);
    Task<Guid> AddBalance(Balance entidade, EF.Record record, Extract extract);
    Task<int> UpdateBalance(Balance entidade, EF.Record record);
    Task<int> UpdateBalance(Balance entidade, EF.Record record, Extract extract);
  }
}
