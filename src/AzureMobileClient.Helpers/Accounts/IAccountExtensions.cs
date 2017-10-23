using System;
using System.Collections.Generic;
using System.Text;

namespace AzureMobileClient.Helpers.Accounts
{
    public static class IAccountExtensions
    {
        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <returns>The string value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static string GetStringValue(this IAccount account, string key, string defaultValue = null)
        {
            string r;
            if (!account.TryGetValue(key, out r))
                r = defaultValue;
            return r;
        }

        /// <summary>
        /// Sets the string value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SetStringValue(this IAccount account, string key, string value = null)
        {
            if (account.ContainsKey(key))
                account[key] = value;
            else
                account.Add(key, value);
        }

        /// <summary>
        /// Gets the date time value.
        /// </summary>
        /// <returns>The date time value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static DateTime? GetDateTimeValue(this IAccount account, string key, DateTime? defaultValue = null)
        {
            DateTime r;

            var str = account.GetStringValue(key);

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
        public static void SetDateTimeValue(this IAccount account, string key, DateTime? value = null)
        {
            if (!value.HasValue)
                account.SetStringValue(key);
            else
            {
                var t = value.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var s = (int)t.TotalSeconds;
                account.SetStringValue(key, s.ToString());
            }
        }

        /// <summary>
        /// Gets the long value.
        /// </summary>
        /// <returns>The long value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static long? GetLongValue(this IAccount account, string key, long? defaultValue = null)
        {
            long r;

            var str = account.GetStringValue(key);

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
        public static void SetLongValue(this IAccount account, string key, long? value = null)
        {
            if (!value.HasValue)
                account.SetStringValue(key);
            else
                account.SetStringValue(key, value.Value.ToString());
        }

        /// <summary>
        /// Gets the bool value.
        /// </summary>
        /// <returns>The bool value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static bool? GetBoolValue(this IAccount account, string key, bool? defaultValue = null)
        {
            bool r;

            var str = account.GetStringValue(key);

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
        public static void SetBoolValue(this IAccount account, string key, bool? value = null)
        {
            if (!value.HasValue)
                account.SetStringValue(key);
            else
                account.SetStringValue(key, value.Value.ToString());
        }
    }
}
