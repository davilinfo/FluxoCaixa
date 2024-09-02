using System.Diagnostics.CodeAnalysis;
using Domain.Account.Commands;

namespace Domain.Account.Validations
{
  [ExcludeFromCodeCoverage]
  internal class RegisterAccountCommandValidation : AccountValidation<RegisterAccountCommand>
  {
    public RegisterAccountCommandValidation() { 
      ValidateAccountNumber();
      ValidateName();
      ValidateEmail();
    }
  }
}
