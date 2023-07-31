using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
  public class GetBalanceRequest
  {
    [Required(ErrorMessage = "Você deve informar o identificador guid da conta")]    
    public required string AccountId { get; set; }
  }
}
