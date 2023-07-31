using Application.Models.ViewModel;
using AutoMapper;
using Domain.EF;
using System.Diagnostics.CodeAnalysis;

namespace Application.AutoMapper
{
  [ExcludeFromCodeCoverage]
  public class DomainToViewModelMapping : Profile
  {
    public DomainToViewModelMapping() { 
      CreateMap<Account, AccountViewModel>();
      CreateMap<Balance, BalanceViewModel>();
      CreateMap<Extract, ExtractViewModel>();
      CreateMap<Record, RecordViewModel>();
    }
  }
}
