using Domain.EF;

namespace Domain.Contract
{
  public interface IRepositoryFluxoCaixa
  {
    Task<Guid> AddBalance(Balance entidade, Record record);
    Task<Guid> AddBalance(Balance entidade, Record record, Extract extract);
    Task<int> UpdateBalance(Balance entidade, Record record);
    Task<int> UpdateBalance(Balance entidade, Record record, Extract extract);
  }
}
