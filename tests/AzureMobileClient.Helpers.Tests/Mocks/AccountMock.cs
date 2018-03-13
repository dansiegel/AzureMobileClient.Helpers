using System;
using AzureMobileClient.Helpers.Accounts;

namespace AzureMobileClient.Helpers.Tests.Mocks
{
    public class AccountMock : Account
    {
        public const string AccessTokenValue = "UserAccessToken";
        public const string IdValue = "UserId";

        public AccountMock(bool isValid = true)
        {
            _isValid = isValid;
            Id = IdValue;
            AccessToken = AccessTokenValue;
        }

        private bool _isValid;
        public override bool IsValid => _isValid;

        public bool? TestBoolean
        {
            get => this.GetBoolValue("test_bool");
            set => this.SetBoolValue("test_bool", value);
        }

        public DateTime? TestDateTime
        {
            get => this.GetDateTimeValue("test_datetime");
            set => this.SetDateTimeValue("test_datetime", value);
        }
    }
}
