using System;

namespace AzureMobileClient.Helpers.Accounts.OAuth
{
    /// <summary>
    /// OAuth2 account.
    /// </summary>
    public interface IOAuth2Account : IAccount
    {
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>The refresh token.</value>
        string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>The type of the token.</value>
        string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>The scope.</value>
        string Scope { get; set; }

        /// <summary>
        /// Gets or sets the access token expires.
        /// </summary>
        /// <value>The access token expires.</value>
        DateTime? AccessTokenExpires { get; set; }

        /// <summary>
        /// Gets or sets the refresh token expires.
        /// </summary>
        /// <value>The refresh token expires.</value>
        DateTime? RefreshTokenExpires { get; set; }
    }
}
