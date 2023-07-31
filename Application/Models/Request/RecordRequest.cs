using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Models.Request
{
  [ExcludeFromCodeCoverage]
  public class RecordRequest
  {
    [Required(ErrorMessage = "Você deve informar o id da conta")]
    public required string AccountId { get; set; }    
    [Required(ErrorMessage = "Você deve informar a descrição do lançamento")]
    public required string Description { get; set; }
    [Required(ErrorMessage = "Você deve informar o tipo de lançamento em C ou D. C=Crédito, D=Débito")]    
    [MaxLength(1)]
    public required string Type { get; set; }
    [Required(ErrorMessage = "Você deve informar o valor do lançamento")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor mínimo para lançar é de 0,01")]
    public double Value { get; set; }
  }
}
