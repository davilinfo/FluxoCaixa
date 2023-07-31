using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Application.AutoMapper
{
  [ExcludeFromCodeCoverage]
  public class RequestToViewModelMapping : Profile
  {
    public RequestToViewModelMapping() {
      CreateMap<AccountRequest, AccountViewModel>();
      CreateMap<GetAccountRequest, AccountViewModel>();
      CreateMap<RecordRequest, RecordViewModel>();
    }    
  }
}
