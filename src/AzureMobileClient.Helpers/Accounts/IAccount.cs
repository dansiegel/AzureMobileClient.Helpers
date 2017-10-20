
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers.Accounts
{
    /// <summary>
    /// Account.
    /// </summary>
    public interface IAccount : IDictionary<string, string>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AzureMobileClient.Helpers.Accounts.IAccount"/> is valid.
        /// </summary>
        /// <value><c>true</c> if is valid; otherwise, <c>false</c>.</value>
        bool IsValid { get; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        string AccessToken { get; set; }

        /// <summary>
        /// Checks the validity.
        /// </summary>
        /// <returns>The validity.</returns>
        Task<bool> CheckValidity();
    }
}