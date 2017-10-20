using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts.OAuth;

namespace AzureMobileClient.Helpers.Http
{
    public class OAuthDelegatingHandler : DelegatingHandler
    {
        const string ZumoAuthHeader = "X-ZUMO-AUTH";

        protected ICloudService<IOAuth2Account> _cloudService { get; }

        /// <summary>
        /// Initializes the <see cref="AuthenticationDelegatingHandler" />
        /// </summary>
        public OAuthDelegatingHandler(ICloudService<IOAuth2Account> cloudService)
        {
            _cloudService = cloudService;
        }

        /// <inheritDoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Clone the request, in case we need to re-issue it
            var clone = await request.CloneHttpRequestMessageAsync();
            // Now do the request
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // The request resulted in a 401 Unauthorized.  We need to do a LoginAsync,
                // which will do the Refresh if appropriate, or ask for credentials if not.
                var user = await _cloudService.LoginAsync();

                // Now, retry the request with the cloned request.  The only thing we have
                // to do is replace the X-ZUMO-AUTH header with the new auth token.
                clone.Headers.Remove(ZumoAuthHeader);
                clone.Headers.Add(ZumoAuthHeader, user.RefreshToken);
                response = await base.SendAsync(clone, cancellationToken);
            }

            return response;
        }
    }
}
