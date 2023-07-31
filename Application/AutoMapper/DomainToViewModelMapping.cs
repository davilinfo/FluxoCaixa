using Application.Models.ViewModel;
using AutoMapper;
using Domain.EF;

namespace Application.AutoMapper
{
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
