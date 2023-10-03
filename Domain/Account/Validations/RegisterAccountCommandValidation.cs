using Domain.Account.Commands;

namespace Domain.Account.Validations
{
  internal class RegisterAccountCommandValidation : AccountValidation<RegisterAccountCommand>
  {
    public RegisterAccountCommandValidation() { 
      ValidateAccountNumber();
      ValidateName();
      ValidateEmail();
    }
  }
}
