using Newtonsoft.Json;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// User Claim
    /// </summary>
    public class UserClaim
    {
        /// <summary>
        /// Type of Claim
        /// </summary>
        [JsonProperty(PropertyName = "typ")]
        public string Type { get; set; }

        /// <summary>
        /// Claim Value
        /// </summary>
        [JsonProperty(PropertyName = "val")]
        public string Value { get; set; }
    }
}