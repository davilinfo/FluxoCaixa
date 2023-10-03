using Domain.Account.Commands;

namespace Domain.Account.Validations
{
  public class UpdateAccountCommandValidation : AccountValidation<UpdateAccountCommand>
  {
    public UpdateAccountCommandValidation() 
    { 
      ValidateAccountNumber();
      ValidateName();
      ValidateEmail();
    }
  }
}
