using Domain.Record.Commands;
using FluentValidation;

namespace Domain.Record.Validations
{
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
    }
    public void ValidateValue()
    {
      RuleFor(v=> v.Value).NotEmpty().WithMessage("Value é obrigatório");
    }


  }
}
