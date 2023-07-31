using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
  public class AccountRequest
  {    
    [Required(ErrorMessage = "O nome do dono da conta de fluxo é obrigatório")]
    [MaxLength(150, ErrorMessage = "Máximo de 150 caracteres")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "O email do dona da conta de fluxo é obrigatório")]
    [EmailAddress(ErrorMessage = "O email é inválido")]
    public required string Email { get; set; }
  }
}
