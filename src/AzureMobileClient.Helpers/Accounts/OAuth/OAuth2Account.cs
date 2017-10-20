using System;

namespace AzureMobileClient.Helpers.Accounts.OAuth
{
    /// <summary>
    /// OAuth2 Account.
    /// </summary>
    public class OAuth2Account : Account, IOAuth2Account
    {
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>The refresh token.</value>
        public string RefreshToken
        {
            get { return GetStringValue("refresh_token"); }
            set { SetStringValue("refresh_token", value); }
        }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>The type of the token.</value>
        public string TokenType
        {
            get { return GetStringValue("token_type"); }
            set { SetStringValue("token_type", value); }
        }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public string Scope
        {
            get { return GetStringValue("scope"); }
            set { SetStringValue("scope", value); }
        }

        /// <summary>
        /// Gets or sets the access token expires.
        /// </summary>
        /// <value>The access token expires.</value>
        public DateTime? AccessTokenExpires
        {
            get { return GetDateTimeValue("expires_in"); }
            set { SetDateTimeValue("expires_in", value); }
        }

        /// <summary>
        /// Gets or sets the refresh token expires.
        /// </summary>
        /// <value>The refresh token expires.</value>
        public DateTime? RefreshTokenExpires
        {
            get { return GetDateTimeValue("refresh_expires_in"); }
            set { SetDateTimeValue("refresh_expires_in", value); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AzureMobileClient.Helpers.Accounts.OAuth.OAuth2Account"/>
        /// is valid.
        /// </summary>
        /// <value><c>true</c> if is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get
            {
                var valid = true;

                // Missing access token AND id token, so no tokens? login again.
                if (string.IsNullOrEmpty(AccessToken))
                    valid = false;

                // Access Token has expiry date and is expired? login again.
                if (AccessTokenExpires.HasValue && AccessTokenExpires.Value.ToUniversalTime() >= DateTime.UtcNow)
                    valid = false;

                return valid;
            }
        }
    }
}
