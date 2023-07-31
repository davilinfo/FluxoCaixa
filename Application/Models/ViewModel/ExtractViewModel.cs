using System.Diagnostics.CodeAnalysis;

namespace Application.Models.ViewModel
{
  [ExcludeFromCodeCoverage]
  public class ExtractViewModel
  {
    public Guid Id { get; set; }
    public double Value { get; set; }
    public DateTime Created { get; set; }    
    public virtual RecordViewModel? IdRecord { get; set; }    
    public required virtual AccountViewModel IdAccount { get; set; }
  }
}
