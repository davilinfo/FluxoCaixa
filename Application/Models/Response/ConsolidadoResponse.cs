using Application.Models.ViewModel;

namespace Application.Models.Response
{
  public class ConsolidadoResponse
  {
    public Guid Id { get; set; }
    public double BalanceValue { get; set; }
    public DateTime Created { get; set; }
    public virtual List<RecordViewModel> Records { get; set; }
    public virtual AccountViewModel IdAccount { get; set; }
  }
}
