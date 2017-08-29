using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureMobileClient.Helpers;
using AzureMobileClient.Helpers.Accounts;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    public class AADLoginProvider : LoginProviderBase
    {
        protected IPublicClientApplication _client { get; }

        protected UIParent _parent { get; }

        protected IAADOptions _options { get; }

        public override string AccountServiceName => throw new NotImplementedException();

        public AADLoginProvider(IPublicClientApplication client, UIParent parent, IAADOptions options, IAccountStore accountStore)
            : base(accountStore)
        {
            _client = client;
            _options = options;
            _parent = parent;
        }

        public override MobileServiceUser RetrieveTokenFromSecureStore()
        {
            var account = AccountStore.FindAnyAccount(AccountServiceName) as AADAccount;
            if (account?.IsValid ?? false)
            {
                return new MobileServiceUser(account.Id)
                {
                    MobileServiceAuthenticationToken = account.AccessToken
                };
            }

            return null;
        }

        public override void StoreTokenInSecureStore(MobileServiceUser user) =>
            SaveAccountInSecureStore(new AADAccount(user.MobileServiceAuthenticationToken));

        public override async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client)
        {
            var accessToken = await LoginADALAsync();
            var zumoPayload = new JObject();
            zumoPayload["access_token"] = accessToken;
            var user = await client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, zumoPayload);
            StoreTokenInSecureStore(user);
            return user;
        }

        /// <summary>
        /// Login via ADAL
        /// </summary>
        /// <returns>(async) token from the ADAL process</returns>
        public async Task<string> LoginADALAsync()
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
            foreach (var user in _client.Users)
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

        protected virtual void Log(Exception exception)
        {

        }
    }
}
