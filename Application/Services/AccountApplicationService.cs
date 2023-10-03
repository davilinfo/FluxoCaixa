using Application.Exception;
using Application.Interfaces;
using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Account.Commands;
using Domain.Contract;
using Domain.EF;
using System.Diagnostics.CodeAnalysis;

namespace Application.Services
{
  [ExcludeFromCodeCoverage]
  public class AccountApplicationService : IApplicationServiceAccount
  {
    private readonly IMapper _mapper;
    private readonly IRepositoryAccount _repositoryAccount;
    const string _emailExistenteMsg = "Já existe uma conta informada com esse email!";
    const string _contaExistenteMsg = "Erro em numeração de conta, tente criar novamente!";
    const string _contaNaoEncontradaMsg = "Conta não encontrada!";
    public AccountApplicationService(IMapper mapper, IRepositoryAccount repositoryAccount)
    {
      _mapper = mapper;
      _repositoryAccount = repositoryAccount;
    }
    public async Task<AccountViewModel> Add(AccountViewModel model)
    {      
      var registerAccount = _mapper.Map<RegisterAccountCommand>(model);

      if (registerAccount.IsValid())
      {
        var entity = registerAccount.CreateAccount();
        HandleValidation(entity);

        var guid = await _repositoryAccount.Add(entity);

        model = _mapper.Map<AccountViewModel>(entity);
      }      

      return model;
    }

    public IEnumerable<AccountViewModel> GetByEmail(string email)
    {
      var result = _repositoryAccount.All().FirstOrDefault(a => a.Email == email);
      var list = new List<AccountViewModel>();
      if (result != null)
      {
        list.Add(_mapper.Map<AccountViewModel>(result));
      }

      return list;
    }

    public async Task<AccountViewModel> GetById(Guid id)
    {
      var result = await _repositoryAccount.GetById(id);
      if (result != null)
        return _mapper.Map<AccountViewModel>(result);
      return null;
    }

    public async Task<AccountViewModel> Update(AccountViewModel model)
    {      
      var actual = _repositoryAccount.All().FirstOrDefault(a => a.Id == model.Id);
      if (actual == null)
      {
        throw new BusinessException(_contaNaoEncontradaMsg);
      }

      var account = _mapper.Map<UpdateAccountCommand>(actual);

      if (account != null && actual.Id == model.Id) 
      { 
        if (account.IsValid())
        {
          actual = account.UpdateCommand(actual);
          await _repositoryAccount.Update(actual);
        }
        else
        {
          throw new BusinessException(_contaNaoEncontradaMsg);
        }
      }           

      model = _mapper.Map<AccountViewModel>(actual);

      return model;
    }

    public async Task<AccountViewModel> AddAsync(AccountRequest request)
    {
      var viewModel = _mapper.Map<AccountViewModel>(request);

      var result = await Add(viewModel);

      return result;
    }

    private void HandleValidation(Account model)
    {
      if (_repositoryAccount.All().Any(a => a.Email == model.Email))
      {
        throw new BusinessException(_emailExistenteMsg);
      }
      if (_repositoryAccount.All().Any(a => a.AccountNumber == model.AccountNumber))
      {
        throw new BusinessException(_contaExistenteMsg);
      }
    }
  }
}
