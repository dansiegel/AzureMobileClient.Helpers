using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers.Accounts
{
    /// <summary>
    /// Account.
    /// </summary>
    public abstract class Account : Dictionary<string, string>, IAccount
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            get { return this.GetStringValue("id"); }
            set { this.SetStringValue("id", value); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.GetStringValue("name"); }
            set { this.SetStringValue("name", value); }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get => this.GetStringValue("access_token");
            set => this.SetStringValue("access_token", value);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AzureMobileClient.Helpers.Accounts.Account"/> is valid.
        /// </summary>
        /// <value><c>true</c> if is valid; otherwise, <c>false</c>.</value>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Gets the Token to be used by the Mobile Service Client
        /// </summary>
        public string MobileServiceClientToken
        {
            get => this.GetStringValue("mobile_service_client_token");
            set => this.SetStringValue("mobile_service_client_token", value);
        }

        /// <summary>
        /// Gets the Expiration of the Token used by the Mobile Service Client
        /// </summary>
        public DateTime MobileServiceClientTokenExpires
        {
            get => this.GetDateTimeValue("mobile_service_client_token_expires") ?? DateTime.Now;
            set => this.SetDateTimeValue("mobile_service_client_token_expires", value);
        }

        /// <summary>
        /// Checks the validity.
        /// </summary>
        /// <returns>The validity.</returns>
        public virtual Task<bool> CheckValidityAsync() => Task.FromResult(IsValid);
    }
}
