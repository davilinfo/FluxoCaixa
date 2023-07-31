using Domain.EF.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.EF
{
  [ExcludeFromCodeCoverage]
  [Table("Balance")]
  public class Balance : GuidEntity
  {
    [Required(ErrorMessage = "Saldo inválido")]
    [Range(0, double.MaxValue, ErrorMessage = "Saldo mínimo é 0! E o máximo suportado é bem grande!")]
    public double Value { get; set; }    
    public DateTime Updated { get; set; }
    public DateTime Created { get; set; }
    [Required(ErrorMessage = "Conta é obrigatório para informar Saldo")]
    [ForeignKey("IdAccount")]
    public required virtual Account IdAccountNavigation { get; set; }
  }
}
