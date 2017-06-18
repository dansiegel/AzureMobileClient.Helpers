using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using AzureMobileClient.Helpers.Accounts;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// ILoginProvider
    /// </summary>
    public interface ILoginProvider
    {
        /// <summary>
        /// Retrieves the Token from the Secure Store
        /// </summary>
        MobileServiceUser RetrieveTokenFromSecureStore();

        /// <summary>
        /// Retrieve's the Account from the Secure Store
        /// </summary>
        /// <returns></returns>
        IAccount RetrieveOAuthAccountFromSecureStore();

        /// <summary>
        /// Stores the Token in the Secure Store
        /// </summary>
        void StoreTokenInSecureStore(MobileServiceUser user);

        void SaveAccountInSecureStore(IAccount account);

        /// <summary>
        /// Removes the Authentication Token from the Secure Store
        /// </summary>
        void RemoveTokenFromSecureStore();

        /// <summary>
        /// Perform Login Action for an unauthenticated user
        /// </summary>
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client);
    }
}