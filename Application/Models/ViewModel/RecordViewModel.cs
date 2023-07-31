using Application.Enum;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.ViewModel
{
  public class RecordViewModel
  {
    public Guid Id { get; set; }
    [Required(ErrorMessage = "A descrição do lançamento é obrigatória")]
    public required string History { get; set; }    
    public DateTime DateTime { get; set; }
    [Required(ErrorMessage = "O tipo do lançamento em C ou D é obrigatório")]
    [MaxLength(1)]
    [EnumDataType(typeof(RecordType))]
    public char Type { get; set; }
    [Required(ErrorMessage = "O valor do lançamento é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor mínimo é 0,01 e máximo é bem grande!")]
    
    public double Value { get; set; }
    
    public virtual required AccountViewModel IdAccountNavigation { get; set; }
  }
}
