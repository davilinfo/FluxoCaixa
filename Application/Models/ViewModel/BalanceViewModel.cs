﻿using System.Diagnostics.CodeAnalysis;

namespace Application.Models.ViewModel
{
  public class BalanceViewModel
  {
    [ExcludeFromCodeCoverage]
    public Guid Id { get; set; }
    public double Value { get; set; }
    public DateTime Updated { get; set; }
    public DateTime Created { get; set; }    
    public required AccountViewModel IdAccountNavigation { get; set; }
  }
}
