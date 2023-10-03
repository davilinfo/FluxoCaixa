using Domain.Account.Validations;

namespace Domain.Account.Commands
{
  public class RegisterAccountCommand : AccountCommand
  {
    public RegisterAccountCommand(long accountNumber, string name, string email)
    {
      AccountNumber = accountNumber == 0 ? new Random().NextInt64() : accountNumber;
      Name = name;
      Email = email;
    }

    public override bool IsValid()
    {
      ValidationResult = new RegisterAccountCommandValidation().Validate(this);
      return ValidationResult.IsValid;
    }

    public Domain.EF.Account CreateAccount()
    {
      return new EF.Account(AccountNumber, Name, Email);
    }
  }
}
