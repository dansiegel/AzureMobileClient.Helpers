using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers.Tests.Mocks
{
    public class LoginProviderMock : LoginProviderBase<AccountMock>
    {
        public LoginProviderMock()
        {
        }

        public override string AccountServiceName => "AzureMobileClientTestProvider";

        public override Task<AccountMock> LoginAsync(IMobileServiceClient client)
        {
            return Task.FromResult(new AccountMock());
        }

        protected override AccountMock CreateAccountFromToken(string token, string refreshToken = null)
        {
            var acct = new AccountMock();
            acct.AccessToken = token;
            return acct;
        }
    }
}
