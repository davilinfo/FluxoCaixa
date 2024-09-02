using AutoMapper.Configuration.Annotations;
using Castle.Components.DictionaryAdapter.Xml;
using Domain.Account.Commands;
using Domain.EF;
using Domain.Record.Commands;

namespace UnitTests{
    [TestClass]
    public class DomainCommandsUnitTest{        
#region Record
#region Mutation tests
        [TestMethod]
        public void RegisterRecordCommand_ValueIsNotCreditAndDebit_ShouldFail_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Tipo de registro em conta";
            var accType = 'A';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(false, result);
            Assert.IsTrue(registerRecordCommand.ValidationResult.Errors.Count > 0);
        }
        [TestMethod]
        public void RegisterRecordCommand_ValueIsNotCreditAndDebit2_ShouldFail_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Tipo de registro em conta";
            var accType = '3';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(false, result);
            Assert.IsTrue(registerRecordCommand.ValidationResult.Errors.Count > 0);
        }
        [TestMethod]
        public void RegisterRecordCommand_HistoryEmpty_ShouldFail_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "";
            var accType = 'C';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(false, result);
            Assert.IsTrue(registerRecordCommand.ValidationResult.Errors.Count > 0);
        }
#endregion

        [TestMethod]
        public void RegisterRecordCommand_ValueIsCredit_ShouldSucceed_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Tipo de registro em conta";
            var accType = 'C';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void RegisterRecordCommand_ValueIsCreditNonCapitals_ShouldSucceed_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Tipo de registro em conta";
            var accType = 'c';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void RegisterRecordCommand_ValueIsDebit_ShouldSucceed_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Tipo de registro em conta";
            var accType = 'D';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void RegisterRecordCommand_ValueIsDebitNonCapitals_ShouldSucceed_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Tipo de registro em conta";
            var accType = 'd';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();            

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void RegisterRecordCommand_IsThat_UnitTest(){
            var mockAccount = Mock.Of<Domain.EF.Account>();
            
            var history = "Abertura de conta";
            var accType = 'C';
            var value = 50D;

            var registerRecordCommand = new RegisterRecordCommand(history, accType, value, mockAccount);
            var result = registerRecordCommand.IsValid();
            var record = registerRecordCommand.CreateRecord();

            Assert.IsInstanceOfType(registerRecordCommand, typeof(RegisterRecordCommand));
            Assert.AreEqual(true, result);
            Assert.IsInstanceOfType(record, typeof(Domain.EF.Record));
        }
#endregion Record

#region Account
        [TestMethod]
        public void RegisterAccountCommand_Email1_ShouldFail_UnitTest(){                        
            long accountNumber = 0;
            var name = "Conta de crédito";
            var email = "teste";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid();            

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(false, result);
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count > 0);
        }
        [TestMethod]
        public void RegisterAccountCommand_Email2_ShouldFail_UnitTest(){                        
            long accountNumber = 0;
            var name = "Conta de crédito";
            var email = "teste@";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid();            

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(false, result);
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count > 0);
        }              
        [TestMethod]
        public void RegisterAccountCommand_Email3_ShouldSucceed_UnitTest(){                        
            long accountNumber = 0;
            var name = "Conta de crédito";
            var email = "teste@gmail.com";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid();            

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(true, result);            
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count == 0);
        }
        [TestMethod]
        public void RegisterAccountCommand_NameEmpty_ShouldFail_UnitTest(){                        
            long accountNumber = 0;
            var name = "";
            var email = "teste@gmail.com";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid();            

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(false, result);   
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count > 0);         
        }
        [TestMethod]
        public void RegisterAccountCommand_IsThat_ShouldSucceed_UnitTest(){                        
            long accountNumber = 0;
            var name = "Conta de crédito";
            var email = "teste@gmail.com";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid(); 
            var account = registerAccountCommand.CreateAccount();           

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(true, result);   
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count == 0);
            Assert.IsInstanceOfType(account, typeof(Domain.EF.Account));
        }
        [TestMethod]
        public void RegisterAccountCommand_IsThat2_ShouldSucceed_UnitTest(){                        
            long accountNumber = 0;
            var name = "Conta de débito";
            var email = "teste@gmail.com.br";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid(); 
            var account = registerAccountCommand.CreateAccount();           

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(true, result);   
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count == 0);
            Assert.IsInstanceOfType(account, typeof(Domain.EF.Account));
        }
        [TestMethod]
        public void UpdateAccountCommand_IsThat_ShouldSucceed_UnitTest(){   
                                
            long accountNumber = 452187;
            var name = "Conta de crédito e débito";
            var email = "teste@gmail.com.br";

            var registerAccountCommand = new RegisterAccountCommand(accountNumber, name, email);
            var result = registerAccountCommand.IsValid(); 
            var account = registerAccountCommand.CreateAccount();

            var updateAccountCommand = new UpdateAccountCommand(account.Id, accountNumber, name, email);
            var resultUpdate = updateAccountCommand.IsValid(); 
            var updated = updateAccountCommand.UpdateCommand(account);           

            Assert.IsInstanceOfType(registerAccountCommand, typeof(RegisterAccountCommand));
            Assert.AreEqual(true, result);             
            Assert.IsTrue(registerAccountCommand.ValidationResult.Errors.Count == 0);
            Assert.IsInstanceOfType(account, typeof(Domain.EF.Account));

            Assert.IsInstanceOfType(updateAccountCommand, typeof(UpdateAccountCommand));
            Assert.AreEqual(true, resultUpdate);
            Assert.IsTrue(updateAccountCommand.ValidationResult.Errors.Count == 0);
            Assert.IsInstanceOfType(updated, typeof(Domain.EF.Account));
        }
#endregion
    }
}