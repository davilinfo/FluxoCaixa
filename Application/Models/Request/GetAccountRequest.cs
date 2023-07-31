using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
  public class GetAccountRequest
  {
    [Required(ErrorMessage = "O email do dona da conta de fluxo é obrigatório")]
    [EmailAddress(ErrorMessage = "O email é inválido")]
    public required string Email { get; set; }
  }
}
