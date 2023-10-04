using Domain.Record.Commands;

namespace Domain.Record.Validations
{
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
