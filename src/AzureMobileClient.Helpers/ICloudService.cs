using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// ICloudService
    /// </summary>
    public interface ICloudService<TAccount>
        where TAccount : IAccount
    {
        /// <summary>
        /// The MobileServiceClient used by the ICloud Service
        /// </summary>
        IMobileServiceClient Client { get; }

        /// <summary>
        /// Logs the user in asynchronously
        /// </summary>
        Task<TAccount> LoginAsync();

        /// <summary>
        /// Logs the user out asynchronously
        /// </summary> 
        Task LogoutAsync();

        /// <summary>
        /// Performs the login for a user with specified credentials
        /// </summary>
        Task LoginAsync(User user);

        /// <summary>
        /// Gets the AppServiceIdentity asynchronously
        /// </summary>
        Task<AppServiceIdentity> GetIdentityAsync();
    }
}