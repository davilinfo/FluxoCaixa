using System.Diagnostics.CodeAnalysis;
using Domain.Account.Commands;

namespace Domain.Account.Validations
{
  [ExcludeFromCodeCoverage]
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
