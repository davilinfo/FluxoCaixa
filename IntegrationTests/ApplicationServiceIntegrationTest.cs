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

namespace IntegrationTests
{
    [TestClass]
    public class ApplicationServiceIntegrationTest
    {    
        public static IConfigurationRoot GetTestConfiguration()
        {                       
            var directory =  Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;
            return new ConfigurationBuilder()                
                .SetBasePath(directory)
                .AddJsonFile(@"appsettings.json")                
                .Build();
        }

        public static IMapper GetAutoMapper(){
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(DomainToViewModelMapping));
            services.AddAutoMapper(typeof(RequestToViewModelMapping));
            services.AddAutoMapper(typeof(ViewModelToDomainMapping));
            var serviceProvider = services.BuildServiceProvider();

            var mapper = serviceProvider.GetRequiredService<IMapper>();
            return mapper;
        }

        [TestMethod]
        public async Task  GetBalanceByAccountId_TestMethod()
        {
            var autoMapper = GetAutoMapper();
            var repositoryAccount = new RepositoryAccount(GetTestConfiguration());
            var repositoryBalance = new RepositoryBalance(GetTestConfiguration());                        

            var service = new BalanceApplicationService(autoMapper, repositoryAccount, repositoryBalance);
            var account = repositoryAccount.All().First();
            var result = await service.GetByAccountId(account.Id);
            
            Assert.IsNotNull(result.Id);        
        }     

        [TestMethod]
        public void  GetAccountByEmail_TestMethod()
        {
            var autoMapper = GetAutoMapper();
            var repositoryAccount = new RepositoryAccount(GetTestConfiguration());                                   

            var service = new AccountApplicationService(autoMapper, repositoryAccount);
            var result = service.GetByEmail(@"davinet@live.com");
            
            Assert.IsNotNull(result);        
        }           
    }
}