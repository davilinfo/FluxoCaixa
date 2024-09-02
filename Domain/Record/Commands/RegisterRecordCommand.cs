using System.Diagnostics.CodeAnalysis;
using Domain.Record.Validations;

namespace Domain.Record.Commands
{
  [ExcludeFromCodeCoverage]
  public class RegisterRecordCommand : RecordCommand
  {
    public RegisterRecordCommand(string history, char type, double value, Domain.EF.Account idAccountNavigation) 
    {
      this.DateTime = Created;
      this.History = history;
      this.Type = type;
      this.Value = value;
      this.IdAccountNavigation = idAccountNavigation;
    }
    public override bool IsValid()
    {
      this.DateTime = Created;
      ValidationResult = new RegisterRecordCommandValidation().Validate(this);
      return ValidationResult.IsValid;
    }

    public Domain.EF.Record CreateRecord()
    {
      return new EF.Record(History, DateTime, Type, Value);
    }
  }
}
