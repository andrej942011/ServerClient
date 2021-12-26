using ClientServerLib;
using ClientServerLib.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectAccountTest
{
    [TestClass]
    public class UnitTestClient
    {
        Account accountRegistration;
        const string ipAddress = "127.0.0.1";
        [TestMethod]
        public void CreateAccountServer1()
        {
            accountRegistration = new Account();
            accountRegistration.Unique_Id = "User -1";
            //accountRegistration.UniqueIdGeneration();
            //accountRegistration.UniqueCodeGeneration();

            Assert.IsTrue(RegistrationAccount(accountRegistration, -1));
        }

        [TestMethod]
        public void SendAMaessageAccountISTrue()
        {
            CreateAccountServer1();
            bool outZ = SendAMaessageAccount(accountRegistration, -1);

            Assert.IsTrue(outZ);
        }

        [TestMethod]
        public void SendAMaessageAccountISFalse()
        {
            var notRegisteredAccount = new Account();
            notRegisteredAccount.UniqueIdGeneration();
            notRegisteredAccount.UniqueCodeGeneration();

            ClientPort8001 clientPort8001 = new ClientPort8001(ipAddress, 8001);
            notRegisteredAccount.Message = "tutu";
            clientPort8001.SendMessage(notRegisteredAccount);
            Assert.IsTrue(clientPort8001.Status == "Ошибка авторизации неверен id или код");
        }

        int count = 50;
        Account[] accountsRegistration;
        Task<bool>[] tasksRegistration;
        Task<bool>[] tasksMaessage;

        [TestMethod]
        public void RegistrationOf50Accounts()
        {
            RegistrationAccountsAsync();

            var countR = (from status in tasksRegistration
                          where status.Result == true
                          select status.Result).ToArray();

            if (countR.Length == 0)
                Assert.Fail();
        }

        [TestMethod]
        public void SendAMessagesFrom50Accounts()
        {
            RegistrationAccountsAsync();

            var countR = (from status in tasksRegistration
                          where status.Result == true
                          select status.Result).ToArray();

            if (countR.Length == count)
            {
                //произведем отправку сообщений
                SendAMaessagesAsync();

                var countSend = (from send in tasksMaessage
                                 where send.Result == false
                                 select send.Result).ToArray();

                if (countSend.Length > 0)
                    Assert.Fail($"Ошибка отправки на колличестве аккаунтов {countSend.Length}");
            }
            else
            {
                Assert.Fail("Ошибка создания аккаунтов");
            }
        }

        private async void SendAMaessagesAsync()
        {
            tasksMaessage = new Task<bool>[count];

            for (int i = 0; i < count; i++)
            {
                var temp = accountsRegistration[i];
                var item = i;
                tasksMaessage[i] = Task.Run(() => SendAMaessageAccount(temp, item));
            }
            await Task.WhenAll(tasksMaessage);
        }

        private async void RegistrationAccountsAsync()
        {
            accountsRegistration = new Account[count];
            tasksRegistration = new Task<bool>[count];

            for (int i = 0; i < count; i++)
            {
                accountsRegistration[i] = new Account();
                accountsRegistration[i].Unique_Id = $"id {i}";//.UniqueIdGeneration();
                var temp = accountsRegistration[i];
                var item = i;

                tasksRegistration[i] = Task.Run(() => RegistrationAccount(temp, item));
            }
            await Task.WhenAll(tasksRegistration);
        }
        private bool RegistrationAccount(Account accountRegistration, int id)
        {
            try
            {
                ClientPort8000 clientPort8000 = new ClientPort8000(ipAddress, 8000);

                if(id == -1)
                    accountRegistration = clientPort8000.RegistrationAccount(accountRegistration);
                else
                    accountsRegistration[id] = clientPort8000.RegistrationAccount(accountRegistration);

                return clientPort8000.Status == "OK";
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        private bool SendAMaessageAccount(Account account, int id)
        {
            try
            {
                ClientPort8001 clientPort8001 = new ClientPort8001(ipAddress, 8001);
                account.Message = $"tutu {id}";
                if(id == -1)
                    clientPort8001.SendMessage(account);
                else 
                    accountsRegistration[id] = clientPort8001.SendMessage(account);


                bool outZ = false;
                if (clientPort8001.Status == "Сообщение получено")
                    outZ = true;
                else if (clientPort8001.Status == "Ошибка авторизации неверен id или код")
                    outZ = false;

                return outZ;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
