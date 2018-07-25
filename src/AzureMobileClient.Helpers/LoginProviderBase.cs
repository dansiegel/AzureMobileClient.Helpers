using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base construct for the <see cref="ILoginProvider{TAccount}"/>
    /// </summary>
    /// <typeparam name="TAccount"></typeparam>
    public abstract class LoginProviderBase<TAccount> : ILoginProvider<TAccount>
        where TAccount : IAccount
    {
        /// <summary>
        /// The Acount Service Name
        /// </summary>
        public abstract string AccountServiceName { get; }

        /// <summary>
        /// The <see cref="AccountStore"/>
        /// </summary>
        protected AccountStore AccountStore { get; }

        /// <summary>
        /// Creates a new instance of <see cref="LoginProviderBase{TAccount}"/>
        /// </summary>
        public LoginProviderBase()
        {
            AccountStore = AccountStore.Create();
        }

        /// <summary>
        /// Performs the Login
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public abstract Task<TAccount> LoginAsync(IMobileServiceClient client);

        /// <summary>
        /// Creates a Account from a given token
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="mobileServiceClientToken">The mobile service client token</param>
        /// <returns></returns>
        protected abstract TAccount CreateAccountFromToken(string token, string mobileServiceClientToken = null);

        /// <summary>
        /// Removes the token from the secure store
        /// </summary>
        /// <returns></returns>
        public virtual async Task RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService(AccountServiceName);
            foreach (var account in accounts)
                await AccountStore.DeleteAsync(account, AccountServiceName);
        }

        /// <summary>
        /// Retrieves the OAuth Account from the Secure Store
        /// </summary>
        /// <returns>The Account</returns>
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

        /// <summary>
        /// Saves the Account in the secure store
        /// </summary>
        /// <param name="account"></param>
        public virtual void SaveAccountInSecureStore(TAccount account)
        {
            var authAccount = new Xamarin.Auth.Account(account.Id, account);
            this.AccountStore.Save(authAccount, AccountServiceName);
        }

        /// <summary>
        /// Handles logging for any exceptions thrown within the <see cref="LoginProviderBase{TAccount}"/>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to handle</param>
        protected virtual void Log(Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}
