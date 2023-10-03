using FluentValidation.Results;

namespace Domain.Commands
{
  public abstract class Command
  {
    public DateTime Created { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
      Created = DateTime.Now;
    }

    public abstract bool IsValid();
  }
}
