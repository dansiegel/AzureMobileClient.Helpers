using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    /// <summary>
    /// An extension of the <see cref="AADLoginProvider{TAccount}"/> using an <see cref="AADAccount"/>
    /// </summary>
    public abstract class AADLoginProvider : AADLoginProvider<AADAccount>
    {
        /// <summary>
        /// Creates an instance of the <see cref="AADLoginProvider"/>
        /// </summary>
        /// <param name="client">The <see cref="IPublicClientApplication"/></param>
        /// <param name="parent">The <see cref="UIParent"/>. Only used on Android</param>
        /// <param name="options">The <see cref="IAADOptions"/></param>
        public AADLoginProvider(IPublicClientApplication client, UIParent parent, IAADOptions options)
            : base(client, parent, options)
        {
        }

        /// <summary>
        /// Creates a <see cref="AADAccount"/> based on the provided token.
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="mobileServiceClientToken">The mobile service client token</param>
        /// <returns></returns>
        protected override AADAccount CreateAccountFromToken(string token, string mobileServiceClientToken = null) =>
            new AADAccount(token)
            {
                MobileServiceClientToken = mobileServiceClientToken
            };
    }

    /// <summary>
    /// Provides a generic <see cref="ILoginProvider{TAccount}"/> for Azure Active Directory
    /// </summary>
    /// <typeparam name="TAccount"></typeparam>
    public abstract class AADLoginProvider<TAccount> : LoginProviderBase<TAccount>
        where TAccount : AADAccount
    {
        /// <summary>
        /// The <see cref="IPublicClientApplication"/>
        /// </summary>
        protected IPublicClientApplication _client { get; }

        /// <summary>
        /// The <see cref="UIParent"/> used by Android
        /// </summary>
        protected UIParent _parent { get; }

        /// <summary>
        /// The <see cref="IAADOptions"/>
        /// </summary>
        protected IAADOptions _options { get; }

        /// <summary>
        /// Creates an instance of the <see cref="AADLoginProvider{TAccount}"/>
        /// </summary>
        /// <param name="client">The <see cref="IPublicClientApplication"/></param>
        /// <param name="parent">The <see cref="UIParent"/></param>
        /// <param name="options">The <see cref="IAADOptions"/></param>
        public AADLoginProvider(IPublicClientApplication client, UIParent parent, IAADOptions options)
            : base()
        {
            _client = client;
            _options = options;

#if MONOANDROID
            // Only MonoAndroid uses the UI Parent
            _parent = parent;
#endif
        }

        /// <summary>
        /// Performs the login with the <see cref="IMobileServiceClient"/>
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override async Task<TAccount> LoginAsync(IMobileServiceClient client)
        {
            var account = await RetrieveOAuthAccountFromSecureStore();

            if (account == null)
                return await LoginUnknownUserAsync(client);

            return await LoginFromAccountAsync(account, client);
        }

        /// <summary>
        /// Logs in with a specified <see cref="AADAccount"/>
        /// </summary>
        /// <param name="account">The <see cref="AADAccount"/> to use</param>
        /// <param name="client">The <see cref="IMobileServiceClient"/></param>
        /// <returns></returns>
        protected virtual async Task<TAccount> LoginFromAccountAsync(TAccount account, IMobileServiceClient client)
        {
            if (account.IsValid && account.MobileServiceClientTokenExpires >= DateTime.Now.AddMinutes(30))
            {
                if (client.CurrentUser == null)
                {
                    var claims = new JwtSecurityToken(account.AccessToken).Claims;
                    client.CurrentUser = new MobileServiceUser(claims.FirstOrDefault(c => c.Type == "oid").Value)
                    {
                        MobileServiceAuthenticationToken = account.MobileServiceClientToken
                    };
                }

                return account;
            }
            else if (client.CurrentUser != null)
            {
                var user = await client.RefreshUserAsync();
                account.MobileServiceClientToken = user.MobileServiceAuthenticationToken;
                SaveAccountInSecureStore(account);
                return account;
            }
            else if (account.IsValid)
            {
                account.MobileServiceClientToken = await AuthenticateMobileClientAsync(client, account.AccessToken);
                SaveAccountInSecureStore(account);
                return account;
            }

            return await LoginUnknownUserAsync(client);
        }

        /// <summary>
        /// Logs in with an unknown users.
        /// </summary>
        /// <param name="client">The <see cref="IMobileServiceClient"/></param>
        /// <returns></returns>
        protected virtual async Task<TAccount> LoginUnknownUserAsync(IMobileServiceClient client)
        {
            var accessToken = await LoginADALAsync();
            var mobileServiceClientToken = await AuthenticateMobileClientAsync(client, accessToken);
            var account = CreateAccountFromToken(accessToken, mobileServiceClientToken);
            SaveAccountInSecureStore(account);
            return account;
        }

        /// <summary>
        /// Authenticates given a specified token
        /// </summary>
        /// <param name="client">The <see cref="IMobileServiceClient"/></param>
        /// <param name="aadToken">The token</param>
        /// <returns></returns>
        protected virtual async Task<string> AuthenticateMobileClientAsync(IMobileServiceClient client, string aadToken)
        {
            var zumoPayload = new JObject
            {
                ["access_token"] = aadToken
            };
            var user = await client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, zumoPayload);
            return user.MobileServiceAuthenticationToken;
        }

        /// <summary>
        /// Login via ADAL
        /// </summary>
        /// <returns>(async) token from the ADAL process</returns>
        protected virtual async Task<string> LoginADALAsync()
        {
            try
            {
                var clientUser = GetUserByPolicy(_options.Policy);
                _client.RedirectUri = _options.RedirectUri;
                AuthenticationResult authenticationResult =
                    clientUser != null ?
                    await _client.AcquireTokenSilentAsync(
                        _options.Scopes,
                        clientUser,
                        _options.Authority,
                        false
                    ) :
                    await _client.AcquireTokenAsync(
                        _options.Scopes,
                        string.Empty,
                        UIBehavior.SelectAccount,
                        string.Empty,
                        null,
                        _options.Authority,
                        _parent);

                return authenticationResult.IdToken;
            }
            catch (Exception ex)
            {
                // Checking the exception message 
                // should ONLY be done for B2C
                // reset and not any other error.
                //if(ex.Message.Contains("AADB2C90118"))
                //	OnPasswordReset();
                // Alert if any exception excluding user cancelling sign-in dialog
                //else if(((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
                Log(ex);
                throw;
            }
        }

        private IUser GetUserByPolicy(string policy)
        {
            foreach (var user in _client?.Users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy, StringComparison.OrdinalIgnoreCase)) return user;
            }

            return null;
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }
    }
}
