using Application.Models.ViewModel;
using AutoMapper;
using Domain.EF;
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
    }
  }
}
