using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using AzureMobileClient.Helpers.Accounts;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// ILoginProvider
    /// </summary>
    public interface ILoginProvider<TAccount> where TAccount : IAccount
    {
        /// <summary>
        /// Retrieve's the Account from the Secure Store
        /// </summary>
        /// <returns></returns>
        Task<TAccount> RetrieveOAuthAccountFromSecureStore();

        /// <summary>
        /// Saves the Account in secure store.
        /// </summary>
        /// <returns>The Account in secure store.</returns>
        /// <param name="account">Account.</param>
        Task SaveAccountInSecureStore(TAccount account);

        /// <summary>
        /// Removes the Authentication Token from the Secure Store
        /// </summary>
        Task RemoveTokenFromSecureStore();

        /// <summary>
        /// Perform Login Action for an unauthenticated user
        /// </summary>
        /// <returns>
        /// The Account
        /// </returns>
        Task<TAccount> LoginAsync(IMobileServiceClient client);
    }
}