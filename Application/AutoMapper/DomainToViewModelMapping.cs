using Application.Models.ViewModel;
using AutoMapper;
using Domain.Account.Commands;
using Domain.EF;
using Domain.Record.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Application.AutoMapper
{
  [ExcludeFromCodeCoverage]
  public class DomainToViewModelMapping : Profile
  {
    public DomainToViewModelMapping() { 
      CreateMap<Account, AccountViewModel>();
      CreateMap<Account, UpdateAccountCommand>();
      CreateMap<RegisterAccountCommand, AccountViewModel>();      
      CreateMap<RegisterRecordCommand, RecordViewModel>();
      CreateMap<Balance, BalanceViewModel>();
      CreateMap<Extract, ExtractViewModel>();
      CreateMap<Record, RecordViewModel>();
    }
  }
}
