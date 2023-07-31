namespace Application.Interfaces
{
  public interface IApplicationServiceFluxoCaixa
  {
    Task UpdateBalanceFromRecord(Guid recordId);
  }
}
