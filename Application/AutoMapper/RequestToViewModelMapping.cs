using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Account.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Application.AutoMapper
{
  [ExcludeFromCodeCoverage]
  public class RequestToViewModelMapping : Profile
  {
    public RequestToViewModelMapping() {
      CreateMap<AccountRequest, AccountViewModel>();
      CreateMap<RegisterAccountCommand, AccountViewModel>();
      CreateMap<GetAccountRequest, AccountViewModel>();
      CreateMap<RecordRequest, RecordViewModel>();
    }    
  }
}
