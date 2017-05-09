using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// The App Service Identity
    /// </summary>
    public class AppServiceIdentity
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty(PropertyName = "id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// The Provider's Name
        /// </summary>
        [JsonProperty(PropertyName = "provider_name")]
        public string ProviderName { get; set; }

        /// <summary>
        /// The User's Id
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// User Claims
        /// </summary>
        [JsonProperty(PropertyName = "user_claims")]
        public List<UserClaim> UserClaims { get; set; }
    }
}