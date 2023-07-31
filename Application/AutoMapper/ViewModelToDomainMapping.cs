using Application.Models.ViewModel;
using AutoMapper;
using Domain.EF;

namespace Application.AutoMapper
{
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
