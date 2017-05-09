using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

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
        /// Stores the Token in the Secure Store
        /// </summary>
        void StoreTokenInSecureStore(MobileServiceUser user);

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