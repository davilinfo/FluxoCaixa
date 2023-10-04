using Application.Models.ViewModel;
using AutoMapper;
using Domain.Account.Commands;
using Domain.EF;
using Domain.Record.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Application.AutoMapper
{
  [ExcludeFromCodeCoverage]
  public class ViewModelToDomainMapping : Profile
  {
    public ViewModelToDomainMapping() {
      CreateMap<AccountViewModel, Account>();
      CreateMap<BalanceViewModel, Balance>();
      CreateMap<ExtractViewModel, Extract>();
      CreateMap<RecordViewModel, Record>();
      CreateMap<RecordViewModel, RegisterRecordCommand>();
      CreateMap<AccountViewModel, RegisterAccountCommand>();
    }
  }
}
