using System.Diagnostics.CodeAnalysis;
using Domain.Commands;

namespace Domain.Record.Commands
{
  [ExcludeFromCodeCoverage]
  public abstract class RecordCommand : Command
  {
    public Guid Id { get; protected set; }
    public string History { get; protected set; }    
    public DateTime DateTime { get; protected set; }
        
    public char Type { get; protected set; }
    
    public double Value { get; protected set; }
    
    public virtual Domain.EF.Account IdAccountNavigation { get; protected set; }

  }
}
