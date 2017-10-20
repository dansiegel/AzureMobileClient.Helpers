using System;
using System.IO;

namespace AzureMobileClient.Helpers.Accounts
{
    /// <summary>
    /// Json serializer.
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Deserialize the specified json, throwOnFailure and withTypes.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="json">Json.</param>
        /// <param name="throwOnFailure">If set to <c>true</c> throw on failure.</param>
        /// <param name="withTypes">If set to <c>true</c> with types.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Deserialize<T>(string json, bool throwOnFailure = false, bool withTypes = false)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();

            if (withTypes)
                settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;

            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (Exception ex)
            {
                if (throwOnFailure)
                    throw ex;
            }

            return default(T);
        }

        /// <summary>
        /// Serialize the specified obj, throwOnFailure and withTypes.
        /// </summary>
        /// <returns>The serialize.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="throwOnFailure">If set to <c>true</c> throw on failure.</param>
        /// <param name="withTypes">If set to <c>true</c> with types.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string Serialize<T>(T obj, bool throwOnFailure = false, bool withTypes = false)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();

            if (withTypes)
                settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;

            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
            }
            catch (Exception ex)
            {
                if (throwOnFailure)
                    throw ex;
            }
            return null;
        }
    }
}