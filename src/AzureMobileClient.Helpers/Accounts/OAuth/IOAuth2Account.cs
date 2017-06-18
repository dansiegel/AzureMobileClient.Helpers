using System;

namespace AzureMobileClient.Helpers.Accounts.OAuth
{
    public interface IOAuth2Account : IAccount
    {
        string AccessToken { get; set; }

        string RefreshToken { get; set; }

        string TokenType { get; set; }

        string Scope { get; set; }

        DateTime? AccessTokenExpires { get; set; }

        DateTime? RefreshTokenExpires { get; set; }
    }
}
