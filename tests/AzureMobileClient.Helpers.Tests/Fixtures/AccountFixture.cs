using System;
using Xunit;
using AzureMobileClient.Helpers.Tests.Mocks;

namespace AzureMobileClient.Helpers.Tests.Fixtures
{
    public class AccountFixture
    {
        [Fact]
        public void Account_AccessToken_GetsAndSets()
        {
            var account = new AccountMock();
            Assert.Equal(AccountMock.AccessTokenValue, account.AccessToken);
            Assert.Equal(AccountMock.AccessTokenValue, account["access_token"]);

            var newToken = "AnotherToken";
            account.AccessToken = newToken;
            Assert.Equal(newToken, account.AccessToken);
        }
    }
}