using System.Diagnostics.CodeAnalysis;
using Domain.Record.Commands;

namespace Domain.Record.Validations
{
  [ExcludeFromCodeCoverage]
  public class RegisterRecordCommandValidation : RecordValidation<RegisterRecordCommand>
  {
    public RegisterRecordCommandValidation() 
    { 
      ValidateHistory();
      ValidateDateTime();
      ValidateType();
      ValidateValue();
    }
  }
}
