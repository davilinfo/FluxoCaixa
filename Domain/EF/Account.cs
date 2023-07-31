using Domain.EF.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.EF
{
  [ExcludeFromCodeCoverage]
  [Table("Account")]
  public class Account : GuidEntity
  {
    [Required(ErrorMessage = "AccountNumber é obrigatório")]
    public long AccountNumber { get; set; }
    [Required(ErrorMessage = "Name é obrigatório")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Email é obrigatório")]    
    public required string Email { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    
  }
}
