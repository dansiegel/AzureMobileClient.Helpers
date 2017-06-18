using System;

namespace AzureMobileClient.Helpers.Accounts.OAuth
{
    public class OAuth2Account : Account, IOAuth2Account
    {
        public string AccessToken
        {
            get { return GetStringValue("access_token"); }
            set { SetStringValue("access_token", value); }
        }

        public string RefreshToken
        {
            get { return GetStringValue("refresh_token"); }
            set { SetStringValue("refresh_token", value); }
        }

        public string TokenType
        {
            get { return GetStringValue("token_type"); }
            set { SetStringValue("token_type", value); }
        }

        public string Scope
        {
            get { return GetStringValue("scope"); }
            set { SetStringValue("scope", value); }
        }

        public DateTime? AccessTokenExpires
        {
            get { return GetDateTimeValue("expires_in"); }
            set { SetDateTimeValue("expires_in", value); }
        }

        public DateTime? RefreshTokenExpires
        {
            get { return GetDateTimeValue("refresh_expires_in"); }
            set { SetDateTimeValue("refresh_expires_in", value); }
        }

        public override bool IsValid
        {
            get
            {
                var valid = true;

                // Missing access token AND id token, so no tokens? login again.
                if (string.IsNullOrEmpty(AccessToken))
                    valid = false;

                // Access Token has expiry date and is expired? login again.
                if (AccessTokenExpires.HasValue && AccessTokenExpires.Value.ToUniversalTime() >= DateTime.UtcNow)
                    valid = false;

                return valid;
            }
        }
    }
}
