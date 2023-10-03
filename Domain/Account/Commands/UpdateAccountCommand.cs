using Domain.Account.Validations;

namespace Domain.Account.Commands
{
  public class UpdateAccountCommand : AccountCommand
  {
    public UpdateAccountCommand(Guid id, long accountNumber, string name, string email)
    {
      Id = id;
      AccountNumber = accountNumber;
      Name = name;
      Email = email;
    }

    public override bool IsValid()
    {
      ValidationResult = new UpdateAccountCommandValidation().Validate(this);
      return ValidationResult.IsValid;
    }

    public Domain.EF.Account UpdateCommand(Domain.EF.Account accountEntity)
    {
      return accountEntity.Update(accountEntity.AccountNumber, accountEntity.Name, accountEntity.Email);
    }
  }
}
