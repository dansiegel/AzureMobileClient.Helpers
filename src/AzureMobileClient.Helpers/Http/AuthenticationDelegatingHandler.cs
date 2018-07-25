using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AzureMobileClient.Helpers.Accounts;

namespace AzureMobileClient.Helpers.Http
{
    /// <summary>
    /// Adds Handler to the MobileServiceClient to prompt the user for Authentication
    /// when the response returns an initial 401 Unauthorized response. Then retries the
    /// original call and returns the response.
    /// </summary>
    public class AuthenticationDelegatingHandler<TAccount> : DelegatingHandler
        where TAccount : IAccount
    {
        const string ZumoAuthHeader = "X-ZUMO-AUTH";

        /// <summary>
        /// The <see cref="ICloudService{TAccount}"/> used by the Handler
        /// </summary>
        protected ICloudService<TAccount> _cloudService { get; }

        /// <summary>
        /// Initializes the <see cref="AuthenticationDelegatingHandler{TAccount}" />
        /// </summary>
        public AuthenticationDelegatingHandler(ICloudService<TAccount> cloudService)
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
                clone.Headers.Add(ZumoAuthHeader, user.MobileServiceClientToken);
                response = await base.SendAsync(clone, cancellationToken);
            }

            return response;
        }
    }
}