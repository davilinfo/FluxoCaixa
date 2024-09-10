using AutoMapper;
using Application.AutoMapper;
using Application.Models.ViewModel;
using Application.Services;
using Domain.Contract;
using Domain.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;
using System.Reflection;
using Application.Exception;
using AutoMapper.Configuration.Annotations;
using Azure.Core.Pipeline;
using Azure.Identity;
using Application.Models.Request;

namespace IntegrationTests
{
    [TestClass]
    public class ApplicationServiceIntegrationTest
    {    
        private string  _accountEmail1 = "";
        private string  _accountEmail2 = "";
        private IRepositoryAccount? _repositoryAccount;
        private IRepositoryBalance? _repositoryBalance;
        private IRepositoryFluxoCaixa? _repositoryFluxoCaixa;

        [TestInitialize]
        public async Task Initialize(){            
            _accountEmail1 = "teste@live.com";
            _accountEmail2 = "newEmailTest@live.com";
            _repositoryAccount = new RepositoryAccount(GetTestConfiguration());
            _repositoryBalance = new RepositoryBalance(GetTestConfiguration());
            _repositoryFluxoCaixa = new RepositoryFluxoCaixa(GetTestConfiguration());
            await AddAccount();
        }

        private static IConfigurationRoot GetTestConfiguration()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var directory =  Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return new ConfigurationBuilder()                
                .SetBasePath(directory)
                .AddJsonFile(@"appsettings.json")                
                .Build();
        }

        private static IMapper GetAutoMapper(){
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(DomainToViewModelMapping));
            services.AddAutoMapper(typeof(RequestToViewModelMapping));
            services.AddAutoMapper(typeof(ViewModelToDomainMapping));
            var serviceProvider = services.BuildServiceProvider();

            var mapper = serviceProvider.GetRequiredService<IMapper>();
            return mapper;
        }

        private async Task  AddAccount()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var accountViewModel = new AccountViewModel(){
                AccountNumber = 1234567812,
                Name = "Conta corrente",
                Email = _accountEmail1
            };            
            
            var result = await service.Add(accountViewModel);            
        }

        [TestMethod]
        public async Task  GetBalanceByAccountId_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new BalanceApplicationService(autoMapper, _repositoryAccount, _repositoryBalance);
#pragma warning restore CS8604 // Possible null reference argument.
            var account = _repositoryAccount.All().First();
            var result = await service.GetByAccountId(account.Id);
            
            Assert.IsNotNull(account);        
        }     

        [TestMethod]
        public void  GetAccountByEmail_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var result = service.GetByEmail(_accountEmail1);
            
            Assert.IsNotNull(result);        
        }   

        [TestMethod]
        public void  GetAccountById_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var result = service.GetByEmail(_accountEmail1);
            var account = result.First();
            var searchedAccount = service.GetById(account.Id);
            
            Assert.IsNotNull(searchedAccount);        
        }

        [TestMethod]
        public async Task AddAccount_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var accountViewModel = new AccountViewModel(){
                AccountNumber = new Random().NextInt64(),
                Name = "Conta corrente",
                Email = _accountEmail2
            };
            var result = await service.Add(accountViewModel);            

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddAccount_EmailInexistente_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var accountViewModel = new AccountViewModel(){
                AccountNumber = new Random().NextInt64(),
                Name = "Conta corrente",
                Email = ""
            };
            var result = await service.Add(accountViewModel);            

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == Guid.Empty);
        }        

        [TestMethod]
        public async Task UpdateAccount_TestMethod()
        {
            var autoMapper = GetAutoMapper();    
            var repositoryAccount = new RepositoryAccount(GetTestConfiguration());                                     

            var service = new AccountApplicationService(autoMapper, repositoryAccount);  
            var accounts = service.GetByEmail(_accountEmail1);
            var account = accounts.First();         
            account.Name = "Updated account";
            var result = await service.Update(account);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Updated > result.Created);
        }  

        [TestMethod]
        public async Task AddAccount_ExceptionValidation_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var accountViewModel = new AccountViewModel(){
                AccountNumber = 1234567812,
                Name = "Conta corrente",
                Email = _accountEmail2
            };                                    
            
            var result = await Assert.ThrowsExceptionAsync<BusinessException>(async ()=> await service.Add(accountViewModel) );
                     
            Assert.IsNotNull(result);
        }   

        [TestMethod]
        public async Task AddAccount_ContaExistente_ValidationException_TestMethod()
        {
            var autoMapper = GetAutoMapper();

#pragma warning disable CS8604 // Possible null reference argument.
            var service = new AccountApplicationService(autoMapper, _repositoryAccount);
#pragma warning restore CS8604 // Possible null reference argument.
            var accountViewModel = new AccountViewModel(){
                AccountNumber = 1234567812,
                Name = "Conta corrente",
                Email = _accountEmail2
            };                                    
            
            var result = await Assert.ThrowsExceptionAsync<BusinessException>(async ()=> await service.Add(accountViewModel) );
                     
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CriaLancamento_Credito_TestMethod(){
            var autoMapper = GetAutoMapper();
#pragma warning disable CS8604 // Possible null reference argument.
            var service = new FluxoCaixaApplicationService(autoMapper, _repositoryAccount, _repositoryFluxoCaixa, _repositoryBalance);
#pragma warning restore CS8604 // Possible null reference argument.
            var account = _repositoryAccount.All().First(a=> a.Email == _accountEmail1);
            var recordRequest = new RecordRequest {
                AccountId = account.Id.ToString(),
                Description = "Crédito",
                Type = "C",
                Value = 10000
            };
            var result = await service.AddAsync(recordRequest);            

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CriaLancamento_Debito_Exception_TestMethod(){
            var autoMapper = GetAutoMapper();
#pragma warning disable CS8604 // Possible null reference argument.
            var service = new FluxoCaixaApplicationService(autoMapper, _repositoryAccount, _repositoryFluxoCaixa, _repositoryBalance);
#pragma warning restore CS8604 // Possible null reference argument.
            var account = _repositoryAccount.All().First(a=> a.Email == _accountEmail1);
            var recordRequest = new RecordRequest {
                AccountId = account.Id.ToString(),
                Description = "Débito de 10 bilhões a ser realizado em conta",
                Type = "D",
                Value = 10000000000
            };
            var result = await Assert.ThrowsExceptionAsync<BusinessException>(async ()=> await service.AddAsync(recordRequest));            

            Assert.IsNotNull(result);
        }    
#region Mutation tests
        [TestMethod]
        public async Task CriaLancamento_TipoErrado_NaoCriado_TestMethod(){
            var autoMapper = GetAutoMapper();
#pragma warning disable CS8604 // Possible null reference argument.
            var service = new FluxoCaixaApplicationService(autoMapper, _repositoryAccount, _repositoryFluxoCaixa, _repositoryBalance);
#pragma warning restore CS8604 // Possible null reference argument.
            var account = _repositoryAccount.All().First(a=> a.Email == _accountEmail1);
            var recordRequest = new RecordRequest {
                AccountId = account.Id.ToString(),
                Description = "Tipo lançameto errado",
                Type = "T",
                Value = 50
            };
            var result = await service.AddAsync(recordRequest);            

            Assert.IsTrue(result.Id == Guid.Empty);
        }    
        [TestMethod]
        public async Task CriaLancamento_ContaInexistente_NaoCriado_TestMethod(){
            var autoMapper = GetAutoMapper();
#pragma warning disable CS8604 // Possible null reference argument.
            var service = new FluxoCaixaApplicationService(autoMapper, _repositoryAccount, _repositoryFluxoCaixa, _repositoryBalance);
#pragma warning restore CS8604 // Possible null reference argument.
            var recordRequest = new RecordRequest {
                AccountId = "99999999",
                Description = "Tipo lançameto errado",
                Type = "T",
                Value = 50
            };
            var result = await Assert.ThrowsExceptionAsync<FormatException>(async () => await service.AddAsync(recordRequest));

            Assert.IsNotNull(result);
        }        
#endregion   

        [TestCleanup]
        public async Task Clean(){
            var accountRepository = new RepositoryAccount(GetTestConfiguration());
            var accountTest = accountRepository.All().FirstOrDefault(a => a.Email == _accountEmail1);
            if (accountTest != null){
                var result = await accountRepository.Delete(accountTest.Id);
            }

            var accountTest2 = accountRepository.All().FirstOrDefault(a => a.Email == _accountEmail2);
            if (accountTest2 != null){
                var result = await accountRepository.Delete(accountTest2.Id);
            }
        }
    }
}