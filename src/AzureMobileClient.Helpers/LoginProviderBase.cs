using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using AzureMobileClient.Helpers.Accounts;
using AzureMobileClient.Helpers.Accounts.OAuth;

namespace AzureMobileClient.Helpers
{
    public abstract class LoginProviderBase : ILoginProvider
    {
        public abstract string AccountServiceName { get; }

        public IAccountStore AccountStore { get; }

        public LoginProviderBase(IAccountStore accountStore)
        {
            AccountStore = accountStore;
        }

        public abstract Task<MobileServiceUser> LoginAsync(IMobileServiceClient client);

        public virtual void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccounts(AccountServiceName);
            foreach(var account in accounts)
            {
                AccountStore.DeleteAccount(AccountServiceName, account.Id);
            }
        }

        public virtual MobileServiceUser RetrieveTokenFromSecureStore()
        {
            var account = AccountStore.FindAnyAccount(AccountServiceName) as OAuth2Account;
            if(account?.IsValid ?? false)
            {
                return new MobileServiceUser(account.Id)
                {
                    MobileServiceAuthenticationToken = account.AccessToken
                };
            }

            return null;
        }

        public virtual void StoreTokenInSecureStore(MobileServiceUser user) =>
            SaveAccountInSecureStore(new OAuth2Account()
            {
                Id = user.UserId,
                AccessToken = user.MobileServiceAuthenticationToken
            });

        public virtual IAccount RetrieveOAuthAccountFromSecureStore() =>
            AccountStore.FindAnyAccount(AccountServiceName);

        public virtual void SaveAccountInSecureStore(IAccount account) =>
            AccountStore.SaveAccount(AccountServiceName, account);
    }
}
