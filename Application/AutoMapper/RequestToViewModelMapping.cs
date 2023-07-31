using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;

namespace Application.AutoMapper
{
  public class RequestToViewModelMapping : Profile
  {
    public RequestToViewModelMapping() {
      CreateMap<AccountRequest, AccountViewModel>();
      CreateMap<GetAccountRequest, AccountViewModel>();
      CreateMap<RecordRequest, RecordViewModel>();
    }    
  }
}
