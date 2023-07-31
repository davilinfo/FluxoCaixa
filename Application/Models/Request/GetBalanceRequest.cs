using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Models.Request
{
  [ExcludeFromCodeCoverage]
  public class GetBalanceRequest
  {
    [Required(ErrorMessage = "Você deve informar o identificador guid da conta")]    
    public required string AccountId { get; set; }
  }
}
