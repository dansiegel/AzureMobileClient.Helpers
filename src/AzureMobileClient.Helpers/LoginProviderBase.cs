using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;

namespace AzureMobileClient.Helpers
{
    public abstract class LoginProviderBase<TAccount> : ILoginProvider<TAccount>
        where TAccount : IAccount
    {
        public abstract string AccountServiceName { get; }

        protected AccountStore AccountStore { get; }

        public LoginProviderBase()
        {
            AccountStore = AccountStore.Create();
        }

        public abstract Task<TAccount> LoginAsync(IMobileServiceClient client);

        protected abstract TAccount CreateAccountFromToken(string token, string mobileServiceClientToken = null);

        public virtual async Task RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService(AccountServiceName);
            foreach (var account in accounts)
                await AccountStore.DeleteAsync(account, AccountServiceName);
        }

        public virtual async Task<TAccount> RetrieveOAuthAccountFromSecureStore()
        {
            var accounts = await AccountStore.FindAccountsForServiceAsync(AccountServiceName);
            if (accounts.Any())
            {
                var properties = (IDictionary<string, string>)accounts.FirstOrDefault().Properties;
                return (TAccount)properties;
            }

            return default(TAccount);
        }

        public virtual void SaveAccountInSecureStore(TAccount account)
        {
            var authAccount = new Xamarin.Auth.Account(account.Id, account);
            this.AccountStore.Save(authAccount, AccountServiceName);
        }

        protected virtual void Log(Exception exception)
        {

        }
    }
}
