using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts;
using AzureMobileClient.Helpers.Accounts.OAuth;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    /// <summary>
    /// Provides a base type for an <see cref="IOAuth2Account"/> from Azure Active Directory or 
    /// Azure Active Directory B2C.
    /// </summary>
    public class AADAccount : Dictionary<string, string>, IOAuth2Account
    {
        /// <summary>
        /// Creates an instance of <see cref="AADAccount"/>
        /// </summary>
        public AADAccount()
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="AADAccount"/>
        /// </summary>
        /// <param name="jwt">The JWT</param>
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

        /// <summary>
        /// The User's Object Id
        /// </summary>
        public string Id
        {
            get => this.GetStringValue("oid");
            set => this.SetStringValue("oid", value);
        }

        /// <summary>
        /// The Refresh Token
        /// </summary>
        public string RefreshToken
        {
            get => this.GetStringValue("refresh_token");
            set => this.SetStringValue("refresh_token", value);
        }

        /// <summary>
        /// The Token Type
        /// </summary>
        public string TokenType
        {
            get => this.GetStringValue("token_type");
            set => this.SetStringValue("token_type", value);
        }

        /// <summary>
        /// The Scope
        /// </summary>
        public string Scope
        {
            get => this.GetStringValue("scope");
            set => this.SetStringValue("scope", value);
        }

        /// <summary>
        /// When the <see cref="AccessToken"/> Expires
        /// </summary>
        public DateTime? AccessTokenExpires
        {
            get => this.GetDateTimeValue("exp");
            set => this.SetDateTimeValue("exp", value);
        }

        /// <summary>
        /// When the <see cref="RefreshToken"/> Expires
        /// </summary>
        public DateTime? RefreshTokenExpires
        {
            get => this.GetDateTimeValue("refresh_token_expires");
            set => this.SetDateTimeValue("refresh_token_expires", value);
        }

        /// <summary>
        /// Indicates if the user is new.
        /// </summary>
        public bool IsNew
        {
            get => this.GetBoolValue("newUser") ?? false;
            set => this.SetBoolValue("newUser", value);
        }

        /// <summary>
        /// The user's First Name
        /// </summary>
        public string FirstName
        {
            get => this.GetStringValue("given_name");
            set => this.SetStringValue("given_name", value);
        }

        /// <summary>
        /// The user's Last Name
        /// </summary>
        public string LastName
        {
            get => this.GetStringValue("family_name");
            set => this.SetStringValue("family_name", value);
        }

        /// <summary>
        /// The user's full name
        /// </summary>
        public virtual string Name
        {
            get => $"{FirstName} {LastName}";
            set { }
        }

        /// <summary>
        /// The token issuer
        /// </summary>
        public string Issuer
        {
            get => this.GetStringValue("iss");
            set => this.SetStringValue("iss", value);
        }

        /// <summary>
        /// The Trust Framework Policy
        /// </summary>
        public string TrustFrameworkPolicy
        {
            get => this.GetStringValue("tfp");
            set => this.SetStringValue("tfp", value);
        }

        /// <summary>
        /// When the token was issued
        /// </summary>
        public DateTime? IssuedAt
        {
            get => this.GetDateTimeValue("iat");
            set => this.SetDateTimeValue("iat", value);
        }

        /// <summary>
        /// The Not Before
        /// </summary>
        public DateTime? NotBefore
        {
            get => this.GetDateTimeValue("nbf");
            set => this.SetDateTimeValue("nbf", value);
        }

        /// <summary>
        /// The Subject
        /// </summary>
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
            set
            {
                this.SetStringValue("mobile_service_client_token", value);
                var exp = new JwtSecurityToken(value).Claims.FirstOrDefault(c => c.Type == "exp");
                this["mobile_service_client_token_expires"] = exp.Value;
            }
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
        /// Indicates if the user is still valid based on the JWT.
        /// </summary>
        public virtual bool IsValid => AccessTokenExpires != null &&
                                        DateTime.Now.ToLocalTime() < AccessTokenExpires.Value.ToLocalTime();

        /// <summary>
        /// Checks the validity.
        /// </summary>
        /// <returns>The validity.</returns>
        public virtual Task<bool> CheckValidityAsync() => Task.FromResult(IsValid);
    }
}
