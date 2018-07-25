using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using AzureMobileClient.Helpers.Accounts.OAuth;
using System.Linq;
using AzureMobileClient.Helpers.Accounts;
using AzureMobileClient.Helpers.Http;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// AzureCloudService implementation of <see cref="ICloudService{TAccount}" />
    /// </summary>
    public class AzureCloudService<TAccount> : ICloudService<TAccount>
        where TAccount : IAccount
    {
        /// <summary>
        /// Gets the login provider.
        /// </summary>
        /// <value>The login provider.</value>
        protected ILoginProvider<TAccount> _loginProvider { get; }

        private List<AppServiceIdentity> identities = null;

        /// <summary>
        /// Initializes a new instance of <see cref="AzureCloudService{TAccount}" />
        /// </summary>
        public AzureCloudService(IAzureCloudServiceOptions options, ILoginProvider<TAccount> loginProvider)
        {
            // This is a terrible design, but there isn't a way to update the Mobile Service Client's
            // Handlers after it's been initialized.
            Client = CreateMobileServiceClient(options);

            if (!string.IsNullOrWhiteSpace(options.LoginUriPrefix))
            {
                Client.LoginUriPrefix = options.LoginUriPrefix;
            }

            if (!string.IsNullOrWhiteSpace(options.AlternateLoginHost)
               && Uri.TryCreate(options.AlternateLoginHost, UriKind.Absolute, out Uri altHost))
            {
                Client.AlternateLoginHost = altHost;
            };

            _loginProvider = loginProvider;
        }

        /// <inheritDoc />
        public IMobileServiceClient Client { get; }

        /// <summary>
        /// Creates a new <see cref="IMobileServiceClient"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected virtual IMobileServiceClient CreateMobileServiceClient(IAzureCloudServiceOptions options)
        {
            return new MobileServiceClient(options.AppServiceEndpoint, GetHandlers());
        }

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <returns>The handlers.</returns>
        protected virtual HttpMessageHandler[] GetHandlers()
        {
            return new HttpMessageHandler[] { new AuthenticationDelegatingHandler<TAccount>(this) };
        }

        /// <inheritDoc />
        public virtual async Task<TAccount> LoginAsync()
        {
            var oauthUser = await _loginProvider.RetrieveOAuthAccountFromSecureStore();
            if (oauthUser?.IsValid ?? false)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await Client.RefreshUserAsync();
                    if (refreshed != null)
                    {
                        UpdateRefreshedToken(oauthUser, refreshed);
                    }
                }
                catch
                {

                }
                return oauthUser;
            }

            // We need to ask for credentials at this point
            return await _loginProvider.LoginAsync(Client);
        }

        /// <summary>
        /// Updates the refreshed token.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="mobileServiceUser">Mobile service user.</param>
        protected virtual void UpdateRefreshedToken(TAccount account, MobileServiceUser mobileServiceUser) =>
            account.AccessToken = mobileServiceUser.MobileServiceAuthenticationToken;

        /// <inheritDoc />
        public virtual async Task LogoutAsync()
        {
            if (Client.CurrentUser == null || Client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{Client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add(MobileClientConstants.AuthHeader, Client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            await _loginProvider.RemoveTokenFromSecureStore();

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
            if (Client.CurrentUser == null || Client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            if (identities == null)
            {
                identities = await Client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
            }

            return identities.FirstOrDefault();
        }
    }
}