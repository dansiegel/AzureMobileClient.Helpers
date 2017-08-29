using System;
using System.IdentityModel.Tokens.Jwt;
using AzureMobileClient.Helpers.Accounts;
using AzureMobileClient.Helpers.Accounts.OAuth;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    public class AADAccount : Account, IOAuth2Account
    {
        public AADAccount()
        {
        }

        public AADAccount(string jwt)
        {
            AccessToken = jwt;
            foreach (var claim in new JwtSecurityToken(jwt).Claims)
            {
                Add(claim.Type, claim.Value);
            }
        }

        public new string Id
        {
            get => GetStringValue("oid");
            set => SetStringValue("oid");
        }

        private string _accessToken;
        public string AccessToken
        {
            get => GetStringValue("auth_token");
            set => SetStringValue("auth_token");
        }

        public string RefreshToken
        {
            get => GetStringValue("refresh_token");
            set => SetStringValue("refresh_token");
        }

        public string TokenType
        {
            get => GetStringValue("token_type");
            set => SetStringValue("token_type");
        }

        public string Scope
        {
            get => GetStringValue("scope");
            set => SetStringValue("scope");
        }

        public DateTime? AccessTokenExpires
        {
            get => GetDateTimeValue("exp");
            set => SetDateTimeValue("exp");
        }

        public DateTime? RefreshTokenExpires
        {
            get => GetDateTimeValue("refresh_token_expires");
            set => SetDateTimeValue("refresh_token_expires");
        }

        public bool IsNew
        {
            get => GetBoolValue("newUser") ?? false;
            set => SetBoolValue("newUser");
        }

        public string FirstName
        {
            get => GetStringValue("given_name");
            set => SetStringValue("given_name");
        }

        public string LastName
        {
            get => GetStringValue("family_name");
            set => SetStringValue("family_name");
        }

        public new string Name
        {
            get => $"{FirstName} {LastName}";
        }

        public string Issuer
        {
            get => GetStringValue("iss");
            set => SetStringValue("iss");
        }

        public string TrustFrameworkPolicy
        {
            get => GetStringValue("tfp");
            set => SetStringValue("tfp");
        }

        public DateTime? IssuedAt
        {
            get => GetDateTimeValue("iat");
            set => SetDateTimeValue("iat");
        }

        public DateTime? NotBefore
        {
            get => GetDateTimeValue("nbf");
            set => SetDateTimeValue("nbf");
        }

        public string Subject
        {
            get => GetStringValue("sub");
            set => SetStringValue("sub");
        }

        public override bool IsValid => AccessTokenExpires != null &&
                                        DateTime.Now.ToLocalTime() < AccessTokenExpires.Value.ToLocalTime();
    }
}
