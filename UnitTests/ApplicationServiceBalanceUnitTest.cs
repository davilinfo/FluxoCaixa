using Application.Exception;
using Application.Models.ViewModel;
using Application.Services;
using Domain.Contract;
using Domain.EF;
using System.Runtime.CompilerServices;

namespace UnitTests
{
  [TestClass]
  public class ApplicationServiceBalanceUnitTest
  {
    [TestMethod]
    public async Task  AddTestMethod()
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.NewGuid(), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var balanceViewModel = new BalanceViewModel { Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };
      var balance = new Balance { Id = Guid.NewGuid(), Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = account };
      var newBalance = new BalanceViewModel { Id = balance.Id, Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };

      var mapperSetup = mockMapper.Setup(m => m.Map<Balance>(balanceViewModel)).Returns(balance);
      mapperSetup.Verifiable();

      var mapperSetupAcc = mockMapper.Setup(m => m.Map<Account>(balanceViewModel.IdAccountNavigation)).Returns(account);
      mapperSetupAcc.Verifiable();

      var repoBalance = mockRepositoryBalance.Setup(b => b.Add(balance)).Returns(Task.FromResult(balance.Id));
      repoBalance.Verifiable();

      var mapBalanceAdd = mockMapper.Setup(m => m.Map<BalanceViewModel>(balance)).Returns(newBalance);

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.Add(balanceViewModel);

      mockMapper.VerifyAll();
      mockRepositoryBalance.VerifyAll();

      Assert.IsNotNull(result.Id);
      Assert.AreEqual(result.Id, newBalance.Id);
    }
    
    [DataTestMethod()]
    [DataRow("")]
    public void GetByEmailTestMethod(string email)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      Action a = () =>
      {
        var result = service.GetByEmail(email);
      };

      Assert.ThrowsException<NotImplementedException>(a);
    }

    [DataTestMethod()]
    [DataRow("bad306d6-e5c3-4287-35fe-08db915ba4b5")]
    public async Task GetByAccountIdTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.Parse(guid), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };      
      var balance = new Balance { Id = Guid.NewGuid(), Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = account };
      var balanceViewModel = new BalanceViewModel { Id = balance.Id, Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };

      var mapBalanceGet = mockMapper.Setup(m => m.Map<BalanceViewModel>(balance)).Returns(balanceViewModel);
      mapBalanceGet.Verifiable();

      var repoAccount = mockRepositoryAccount.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult(account));
      repoAccount.Verifiable();

      var repoBalance = mockRepositoryBalance.Setup(b => b.All()).Returns(new List<Balance> { balance }.AsQueryable());
      repoBalance.Verifiable();

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.GetByAccountId(Guid.Parse(guid));

      mockMapper.VerifyAll();
      mockRepositoryAccount.VerifyAll();
      mockRepositoryBalance.VerifyAll();

      Assert.IsNotNull(result.Id);
      Assert.AreEqual(result.IdAccountNavigation.Id, Guid.Parse(guid));
      Assert.AreEqual(result.Id, balance.Id);
    }

    [DataTestMethod()]
    [DataRow("aaa200e0-e5c3-4287-35fe-08db915ba4b5")]
    public async Task GetByAccountIdAccountNotFoundTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.Parse(guid), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      
      var repoAccount = mockRepositoryAccount.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult<Account>(null));
      repoAccount.Verifiable();      

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.GetByAccountId(Guid.Parse(guid));
      
      mockRepositoryAccount.VerifyAll();      

      Assert.IsNull(result);            
    }

    [DataTestMethod()]
    [DataRow("bad306d6-e5c3-4287-35fe-08db915ba4b5")]
    public async Task GetByAccountIdBalanceNotFoundTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.Parse(guid), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
            
      var repoAccount = mockRepositoryAccount.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult(account));
      repoAccount.Verifiable();

      var repoBalance = mockRepositoryBalance.Setup(b => b.All()).Returns(new List<Balance> {  }.AsQueryable());
      repoBalance.Verifiable();

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.GetByAccountId(Guid.Parse(guid));
      
      mockRepositoryAccount.VerifyAll();
      mockRepositoryBalance.VerifyAll();

      Assert.IsNull(result);
    }

    [DataTestMethod()]
    [DataRow("bbb111d1-e5c3-4287-35fe-08db915ba4b5")]
    public async Task GetByIdTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.NewGuid(), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var balance = new Balance { Id = Guid.Parse(guid), Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = account };
      var balanceViewModel = new BalanceViewModel { Id = balance.Id, Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };

      var mapBalanceGet = mockMapper.Setup(m => m.Map<BalanceViewModel>(balance)).Returns(balanceViewModel);
      mapBalanceGet.Verifiable();
      
      var repoBalance = mockRepositoryBalance.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult(balance));
      repoBalance.Verifiable();

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.GetById(Guid.Parse(guid));

      mockMapper.VerifyAll();      
      mockRepositoryBalance.VerifyAll();

      Assert.IsNotNull(result.Id);
      Assert.AreEqual(result.Id, Guid.Parse(guid));
      Assert.AreEqual(result.Id, balance.Id);
    }

    [DataTestMethod()]
    [DataRow("bbb111d1-e5c3-4287-35fe-08db915ba4b5")]
    public async Task GetByIdNotFoundTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();
      
      var repoBalance = mockRepositoryBalance.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult<Balance>(null));
      repoBalance.Verifiable();

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.GetById(Guid.Parse(guid));
      
      mockRepositoryBalance.VerifyAll();

      Assert.IsNull(result);      
    }

    [DataTestMethod()]
    [DataRow("aaa111d1-e5c3-4287-35fe-08db915ba4b5")]
    public async Task UpdateBalanceTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.NewGuid(), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var balance = new Balance { Id = Guid.Parse(guid), Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = account };
      var balanceViewModel = new BalanceViewModel { Id = balance.Id, Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };

      var repoBalance = mockRepositoryBalance.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult<Balance>(balance));
      repoBalance.Verifiable();

      var mapBalanceGet = mockMapper.Setup(m => m.Map<BalanceViewModel>(balance)).Returns(balanceViewModel);
      mapBalanceGet.Verifiable();

      var repoBalanceUpdate = mockRepositoryBalance.Setup(b => b.Update(balance)).Returns(Task.FromResult(1));

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.Update(balanceViewModel);

      Assert.IsNotNull(result);
      Assert.AreEqual(result.Id, balance.Id);
      Assert.AreEqual(result.Id, Guid.Parse(guid));
    }

    [DataTestMethod()]
    [DataRow("bbb111d1-e5c3-4287-35fe-08db915ba4b5")]
    public async Task UpdateBalanceNotFoundTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.Parse(guid), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var balance = new Balance { Id = Guid.NewGuid(), Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = account };
      var balanceViewModel = new BalanceViewModel { Id = balance.Id, Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };

      var repoBalance = mockRepositoryBalance.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult<Balance>(null));      

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.Update(balanceViewModel);      

      Assert.IsNull(result);
    }

    [DataTestMethod()]
    [DataRow("aaa111d1-e5c3-4287-35fe-08db915ba4b5")]
    public void UpdateBalanceNotUpdatedTestMethod(string guid)
    {
      var mockMapper = new Mock<IMapper>();
      var mockRepositoryAccount = new Mock<IRepositoryAccount>();
      var mockRepositoryBalance = new Mock<IRepositoryBalance>();

      var accountViewModel = new AccountViewModel { Id = Guid.NewGuid(), Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var account = new Account { Id = accountViewModel.Id, Name = "teste", Email = "teste@example.com", Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30) };
      var balance = new Balance { Id = Guid.Parse(guid), Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = account };
      var balanceViewModel = new BalanceViewModel { Id = balance.Id, Created = new DateTime(2023, 07, 30), Updated = new DateTime(2023, 07, 30), Value = 0, IdAccountNavigation = accountViewModel };

      var repoBalance = mockRepositoryBalance.Setup(b => b.GetById(Guid.Parse(guid))).Returns(Task.FromResult<Balance>(balance));
      repoBalance.Verifiable();

      var mapBalanceGet = mockMapper.Setup(m => m.Map<BalanceViewModel>(balance)).Returns(balanceViewModel);
      mapBalanceGet.Verifiable();

      var repoBalanceUpdate = mockRepositoryBalance.Setup(b => b.Update(balance)).Returns(Task.FromResult(0));

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);

      Action a = () =>
      {
        var result = service.Update(balanceViewModel).GetAwaiter().GetResult();
      };
      
      Assert.ThrowsException<BusinessException>(a);      
    }
  }
}