using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts;
using AzureMobileClient.Helpers.Accounts.OAuth;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    public class AADAccount : Dictionary<string, string>, IOAuth2Account
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

        ///// <summary>
        ///// Gets or sets the name.
        ///// </summary>
        ///// <value>The name.</value>
        //public string Name
        //{
        //    get { return this.GetStringValue("name"); }
        //    set { this.SetStringValue("name", value); }
        //}

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get => this.GetStringValue("access_token");
            set => this.SetStringValue("access_token", value);
        }

        public string Id
        {
            get => this.GetStringValue("oid");
            set => this.SetStringValue("oid", value);
        }

        public string RefreshToken
        {
            get => this.GetStringValue("refresh_token");
            set => this.SetStringValue("refresh_token", value);
        }

        public string TokenType
        {
            get => this.GetStringValue("token_type");
            set => this.SetStringValue("token_type", value);
        }

        public string Scope
        {
            get => this.GetStringValue("scope");
            set => this.SetStringValue("scope", value);
        }

        public DateTime? AccessTokenExpires
        {
            get => this.GetDateTimeValue("exp");
            set => this.SetDateTimeValue("exp", value);
        }

        public DateTime? RefreshTokenExpires
        {
            get => this.GetDateTimeValue("refresh_token_expires");
            set => this.SetDateTimeValue("refresh_token_expires", value);
        }

        public bool IsNew
        {
            get => this.GetBoolValue("newUser") ?? false;
            set => this.SetBoolValue("newUser", value);
        }

        public string FirstName
        {
            get => this.GetStringValue("given_name");
            set => this.SetStringValue("given_name", value);
        }

        public string LastName
        {
            get => this.GetStringValue("family_name");
            set => this.SetStringValue("family_name", value);
        }

        public virtual string Name
        {
            get => $"{FirstName} {LastName}";
            set { }
        }

        public string Issuer
        {
            get => this.GetStringValue("iss");
            set => this.SetStringValue("iss", value);
        }

        public string TrustFrameworkPolicy
        {
            get => this.GetStringValue("tfp");
            set => this.SetStringValue("tfp", value);
        }

        public DateTime? IssuedAt
        {
            get => this.GetDateTimeValue("iat");
            set => this.SetDateTimeValue("iat", value);
        }

        public DateTime? NotBefore
        {
            get => this.GetDateTimeValue("nbf");
            set => this.SetDateTimeValue("nbf", value);
        }

        public string Subject
        {
            get => this.GetStringValue("sub");
            set => this.SetStringValue("sub", value);
        }

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

        public virtual bool IsValid => AccessTokenExpires != null &&
                                        DateTime.Now.ToLocalTime() < AccessTokenExpires.Value.ToLocalTime();

        /// <summary>
        /// Checks the validity.
        /// </summary>
        /// <returns>The validity.</returns>
        public virtual Task<bool> CheckValidity() => Task.FromResult(IsValid);
    }
}
