using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using AzureMobileClient.Helpers.Accounts;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    public abstract class LoginProviderBase<TAccount> : ILoginProvider<TAccount>
        where TAccount : IAccount
    {
        public abstract string AccountServiceName { get; }

        protected ISecureBlobCache SecureStore { get; }

        public LoginProviderBase()
        {
            SecureStore = BlobCache.Secure;
        }

        public abstract Task<TAccount> LoginAsync(IMobileServiceClient client);

        protected abstract TAccount CreateAccountFromToken(string token, string mobileServiceClientToken = null);

        public virtual async Task RemoveTokenFromSecureStore()
        {
            await SecureStore.InvalidateObject<TAccount>(AccountServiceName);
        }

        public virtual async Task<TAccount> RetrieveOAuthAccountFromSecureStore()
        {
            if(await SecureStore.ContainsKey(AccountServiceName))
                return await SecureStore.GetObject<TAccount>(AccountServiceName);
            return default(TAccount);
        }

        public virtual async Task SaveAccountInSecureStore(TAccount account)
        {
            await SecureStore.InsertObject(AccountServiceName, account);
        }

        protected virtual void Log(Exception exception)
        {

        }
    }
}
