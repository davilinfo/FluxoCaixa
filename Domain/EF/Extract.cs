using Domain.EF.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.EF
{
  [Table("Extract")]
  public class Extract : GuidEntity
  {
    [Required(ErrorMessage = "Saldo inválido")]
    [Range(0,double.MaxValue,ErrorMessage = "Saldo mínimo é 0! E o máximo suportado é bem grande!")]
    public double Value { get; set; }
    public DateTime Created { get; set; }
    [ForeignKey("IdRecord")]
    public virtual Record? IdRecordNavigation { get; set; }
    [Required(ErrorMessage = "Conta é obrigatório para informar Saldo")]
    [ForeignKey("IdAccount")]
    public required virtual Account IdAccountNavigation { get; set; }
  }
}
