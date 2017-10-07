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
        /// Retrieves the Token from the Secure Store
        /// </summary>
        Task<MobileServiceUser> RetrieveTokenFromSecureStore();

        /// <summary>
        /// Retrieve's the Account from the Secure Store
        /// </summary>
        /// <returns></returns>
        Task<TAccount> RetrieveOAuthAccountFromSecureStore();

        /// <summary>
        /// Stores the Token in the Secure Store
        /// </summary>
        Task StoreTokenInSecureStore(MobileServiceUser user);

        Task SaveAccountInSecureStore(TAccount account);

        /// <summary>
        /// Removes the Authentication Token from the Secure Store
        /// </summary>
        Task RemoveTokenFromSecureStore();

        /// <summary>
        /// Perform Login Action for an unauthenticated user
        /// </summary>
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client);
    }
}