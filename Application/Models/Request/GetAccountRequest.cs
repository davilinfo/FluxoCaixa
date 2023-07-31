using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Models.Request
{
  [ExcludeFromCodeCoverage]
  public class GetAccountRequest
  {
    [Required(ErrorMessage = "O email do dona da conta de fluxo é obrigatório")]
    [EmailAddress(ErrorMessage = "O email é inválido")]
    public required string Email { get; set; }
  }
}
