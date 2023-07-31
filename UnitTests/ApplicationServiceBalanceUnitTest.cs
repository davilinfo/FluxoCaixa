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

      var mapBalanceAdd = mockMapper.Setup(m => m.Map<BalanceViewModel>(balance)).Returns(newBalance);

      var service = new BalanceApplicationService(mockMapper.Object, mockRepositoryAccount.Object, mockRepositoryBalance.Object);
      var result = await service.Add(balanceViewModel);

      mockMapper.VerifyAll();

      Assert.IsNotNull(result.Id);
    }
  }
}