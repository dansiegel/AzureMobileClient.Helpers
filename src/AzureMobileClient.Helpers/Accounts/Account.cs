using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers.Accounts
{
    /// <summary>
    /// Account.
    /// </summary>
    public abstract class Account : Dictionary<string, string>, IAccount
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            get { return GetStringValue("id"); }
            set { SetStringValue("id", value); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return GetStringValue("name"); }
            set { SetStringValue("name", value); }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get => GetStringValue("access_token");
            set => SetStringValue("access_token", value);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AzureMobileClient.Helpers.Accounts.Account"/> is valid.
        /// </summary>
        /// <value><c>true</c> if is valid; otherwise, <c>false</c>.</value>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Checks the validity.
        /// </summary>
        /// <returns>The validity.</returns>
        public virtual Task<bool> CheckValidity() => Task.FromResult(IsValid);

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <returns>The string value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        protected virtual string GetStringValue(string key, string defaultValue = null)
        {
            string r;
            if (!TryGetValue(key, out r))
                r = defaultValue;
            return r;
        }

        /// <summary>
        /// Sets the string value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        protected virtual void SetStringValue(string key, string value = null)
        {
            if (ContainsKey(key))
                this[key] = value;
            else
                Add(key, value);
        }

        /// <summary>
        /// Gets the date time value.
        /// </summary>
        /// <returns>The date time value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        protected virtual DateTime? GetDateTimeValue(string key, DateTime? defaultValue = null)
        {
            DateTime r;

            var str = GetStringValue(key);

            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (long.TryParse(str, out long timestamp))
                return new DateTime(1970, 1, 1).AddSeconds(timestamp);

            if (DateTime.TryParse(str, out r))
                return r;

            return defaultValue;
        }

        /// <summary>
        /// Sets the date time value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        protected virtual void SetDateTimeValue(string key, DateTime? value = null)
        {
            if (!value.HasValue)
                SetStringValue(key);
            else
            {
                var t = value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var s = (int)t.TotalSeconds;
                SetStringValue(key, s.ToString());
            }
        }

        /// <summary>
        /// Gets the long value.
        /// </summary>
        /// <returns>The long value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        protected virtual long? GetLongValue(string key, long? defaultValue = null)
        {
            long r;

            var str = GetStringValue(key);

            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (long.TryParse(str, out r))
                return r;

            return defaultValue;
        }

        /// <summary>
        /// Sets the long value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        protected virtual void SetLongValue(string key, long? value = null)
        {
            if (!value.HasValue)
                SetStringValue(key);
            else
                SetStringValue(key, value.Value.ToString());
        }

        /// <summary>
        /// Gets the bool value.
        /// </summary>
        /// <returns>The bool value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        protected virtual bool? GetBoolValue(string key, bool? defaultValue = null)
        {
            bool r;

            var str = GetStringValue(key);

            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (bool.TryParse(str, out r))
                return r;

            return defaultValue;
        }

        /// <summary>
        /// Sets the bool value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        protected virtual void SetBoolValue(string key, bool? value = null)
        {
            if (!value.HasValue)
                SetStringValue(key);
            else
                SetStringValue(key, value.Value.ToString());
        }
    }
}
