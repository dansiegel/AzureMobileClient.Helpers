using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using AzureMobileClient.Helpers.Accounts;
using AzureMobileClient.Helpers.Accounts.OAuth;
using Akavache;
using Akavache.Sqlite3;
using System.Reactive.Linq;
using System.Linq;

namespace AzureMobileClient.Helpers
{
    public abstract class LoginProviderBase<TAccount> : ILoginProvider<TAccount>
        where TAccount : IAccount
    {
        public abstract string AccountServiceName { get; }

        //public IAccountStore AccountStore { get; }

        protected ISecureBlobCache SecureStore { get; }

        public LoginProviderBase()
        {
            SecureStore = BlobCache.Secure;
        }

        public abstract Task<MobileServiceUser> LoginAsync(IMobileServiceClient client);

        public virtual async Task RemoveTokenFromSecureStore()
        {
            await SecureStore.InvalidateObject<OAuth2Account>(AccountServiceName);
        }

        public virtual async Task<MobileServiceUser> RetrieveTokenFromSecureStore()
        {
            var account = await SecureStore.GetObject<OAuth2Account>(AccountServiceName);

            if(account?.IsValid ?? false)
            {
                return new MobileServiceUser(account.Id)
                {
                    MobileServiceAuthenticationToken = account.AccessToken
                };
            }

            return null;
        }

        public abstract Task StoreTokenInSecureStore(MobileServiceUser user);
        //await SaveAccountInSecureStore((TAccount)new OAuth2Account()
        //{
        //    Id = user.UserId,
        //    AccessToken = user.MobileServiceAuthenticationToken
        //});

        public virtual async Task<TAccount> RetrieveOAuthAccountFromSecureStore()
        {
            return await SecureStore.GetObject<TAccount>(AccountServiceName);
        }

        public virtual async Task SaveAccountInSecureStore(TAccount account)
        {
            await SecureStore.InsertObject(AccountServiceName, account);
        }
    }
}
