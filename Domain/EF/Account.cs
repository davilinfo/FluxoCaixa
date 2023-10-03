using Domain.EF.Abstract;
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
    public long AccountNumber { get; private set; }
    [Required(ErrorMessage = "Name é obrigatório")]
    public string Name { get; private set; }
    [Required(ErrorMessage = "Email é obrigatório")]    
    public string Email { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime? Updated { get; private set; }
    
    public Account(long AccountNumber, string Name, string Email)
    {
      this.Id = Guid.NewGuid();
      this.AccountNumber = AccountNumber;
      this.Name = Name;
      this.Email = Email;
      this.Created = DateTime.UtcNow;
      this.Updated = null;
    }

    public Account Update(long AccountNumber, string Name, string Email)
    {      
      this.AccountNumber = AccountNumber;
      this.Name = Name;
      this.Email = Email;      
      this.Updated = DateTime.UtcNow;

      return this;
    }
  }
}
