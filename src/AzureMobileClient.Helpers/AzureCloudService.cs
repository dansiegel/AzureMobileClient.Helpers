using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// AzureCloudService implementation of <see cref="ICloudService" />
    /// </summary>
    public class AzureCloudService : ICloudService
    {
        private ILoginProvider _loginProvider { get; }
        private List<AppServiceIdentity> identities = null;

        /// <summary>
        /// Initializes a new instance of <see cref="AzureCloudService" />
        /// </summary>
        public AzureCloudService(IAzureCloudServiceOptions options, ILoginProvider loginProvider)
        {
            // This is a terrible design, but there isn't a way to update the Mobile Service Client's
            // Handlers after it's been initialized.
            Client = new MobileServiceClient(options.AppServiceEndpoint, new AuthenticationDelegatingHandler(this));

            if(!string.IsNullOrWhiteSpace(options.LoginUriPrefix))
            {
                Client.LoginUriPrefix = options.LoginUriPrefix;
            }

            if(!string.IsNullOrWhiteSpace(options.AlternateLoginHost)
               && Uri.TryCreate(options.AlternateLoginHost, UriKind.Absolute, out Uri altHost))
            {
                Client.AlternateLoginHost = altHost;
            };

            _loginProvider = loginProvider;
        }

        /// <inheritDoc />
        public IMobileServiceClient Client { get; }

        /// <inheritDoc />
        public virtual async Task<MobileServiceUser> LoginAsync()
        {
            Client.CurrentUser = _loginProvider.RetrieveTokenFromSecureStore();
            if(Client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await Client.RefreshUserAsync();
                    if(refreshed != null)
                    {
                        _loginProvider.StoreTokenInSecureStore(refreshed);
                        return refreshed;
                    }
                }
                catch(Exception refreshException)
                {
                    Debug.WriteLine($"Could not refresh token: {refreshException.Message}");
                }
            }

            if(Client.CurrentUser != null && !IsTokenExpired(Client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return Client.CurrentUser;
            }

            // We need to ask for credentials at this point
            await _loginProvider.LoginAsync(Client);
            if(Client.CurrentUser != null)
            {
                // We were able to successfully log in
                _loginProvider.StoreTokenInSecureStore(Client.CurrentUser);
            }
            return Client.CurrentUser;
        }

        /// <inheritDoc />
        public virtual async Task LogoutAsync()
        {
            if(Client.CurrentUser == null || Client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{Client.MobileAppUri}/.auth/logout");
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            _loginProvider.RemoveTokenFromSecureStore();

            // Remove the token from the MobileServiceClient
            await Client.LogoutAsync();
        }

        /// <inheritDoc />
        public virtual Task LoginAsync(User user)
        {
            return Client.LoginAsync("custom", JObject.FromObject(user));
        }

        /// <inheritDoc />
        public virtual async Task<AppServiceIdentity> GetIdentityAsync()
        {
            if(Client.CurrentUser == null || Client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            if(identities == null)
            {
                identities = await Client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
            }

            if(identities.Count > 0)
                return identities[0];
            return null;
        }

        /// <summary>
        /// Validates that the token hasn't yet expired and is still ok to be used
        /// </summary>
        protected virtual bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch(jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }
    }
}