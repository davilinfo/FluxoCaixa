﻿using Domain.Commands;

namespace Domain.Account.Commands
{
  public abstract class AccountCommand : Command
  {
    public Guid Id { get; protected set; }
    public long AccountNumber { get; protected set; }    
    public string Name { get; protected set; }    
    public string Email { get; protected set; }
    public DateTime Created { get; protected set; }
    public DateTime? Updated { get; protected set; }   
  }
}
