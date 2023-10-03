using Domain.Account.Commands;
using FluentValidation;

namespace Domain.Account.Validations
{
  public abstract class AccountValidation<T> : AbstractValidator<T> where T : AccountCommand
  {
    public void ValidateAccountNumber()
    {
      RuleFor(v => v.AccountNumber).NotEmpty().WithMessage("O AccountNumber é obrigatório");
    }
    public void ValidateName()
    {
      RuleFor(v=> v.Name).NotEmpty().WithMessage("O Name é obrigatório");
    }
    public void ValidateEmail()
    {
      RuleFor(v => v.Email).NotEmpty().EmailAddress().WithMessage("O Email é obrigatório");
    }
  }
}
