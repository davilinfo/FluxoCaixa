using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using Domain.Record.Commands;
using FluentValidation;

namespace Domain.Record.Validations
{
  [ExcludeFromCodeCoverage]
  public class RecordValidation<T> : AbstractValidator<T> where T : RecordCommand
  {
    public void ValidateHistory()
    {
      RuleFor(h=> h.History).NotEmpty().WithMessage("History é obrigatório");
    }
    public void ValidateDateTime()
    {
      RuleFor(d=> d.DateTime).NotEmpty().WithMessage("DateTime é obrigatório");
    }
    public void ValidateType()
    {
      RuleFor(t=> t.Type).NotEmpty().WithMessage("Type é obrigatório (C ou D)");
      RuleFor(t=> t.Type).Must(v=> char.ToLower(v) == 'c' || char.ToLower(v) == 'd' ).WithMessage("Type deve ser crédito (C) ou débito (D)");
    }
    public void ValidateValue()
    {
      RuleFor(v=> v.Value).NotEmpty().WithMessage("Value é obrigatório");
    }


  }
}
