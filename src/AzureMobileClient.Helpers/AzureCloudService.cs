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
        private IMobileServiceClient _client { get; }
        private ILoginProvider _loginProvider { get; }
        private List<AppServiceIdentity> identities = null;

        /// <summary>
        /// Initializes a new instance of <see cref="AzureCloudService" />
        /// </summary>
        public AzureCloudService(IAzureCloudServiceOptions options, ILoginProvider loginProvider)
        {
            // This is a terrible design, but there isn't a way to update the Mobile Service Client's
            // Handlers after it's been initialized.
            _client = new MobileServiceClient(options.AppServiceEndpoint, new AuthenticationDelegatingHandler(this));
            _loginProvider = loginProvider;
        }

        /// <inheritDoc />
        public virtual async Task<MobileServiceUser> LoginAsync()
        {
            _client.CurrentUser = _loginProvider.RetrieveTokenFromSecureStore();
            if (_client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await _client.RefreshUserAsync();
                    if (refreshed != null)
                    {
                        _loginProvider.StoreTokenInSecureStore(refreshed);
                        return refreshed;
                    }
                }
                catch (Exception refreshException)
                {
                    Debug.WriteLine($"Could not refresh token: {refreshException.Message}");
                }
            }

            if (_client.CurrentUser != null && !IsTokenExpired(_client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return _client.CurrentUser;
            }

            // We need to ask for credentials at this point
            await _loginProvider.LoginAsync(_client);
            if (_client.CurrentUser != null)
            {
                // We were able to successfully log in
                _loginProvider.StoreTokenInSecureStore(_client.CurrentUser);
            }
            return _client.CurrentUser;
        }

        /// <inheritDoc />
        public virtual async Task LogoutAsync()
        {
            if (_client.CurrentUser == null || _client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{_client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", _client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            _loginProvider.RemoveTokenFromSecureStore();

            // Remove the token from the MobileServiceClient
            await _client.LogoutAsync();
        }

        /// <inheritDoc />
        public virtual Task LoginAsync(User user)
        {
            return _client.LoginAsync("custom", JObject.FromObject(user));
        }

        /// <inheritDoc />
        public virtual async Task<AppServiceIdentity> GetIdentityAsync()
        {
            if (_client.CurrentUser == null || _client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            if (identities == null)
            {
                identities = await _client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
            }

            if (identities.Count > 0)
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
            switch (jwt.Length % 4)
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