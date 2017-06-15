using System;
using System.IO;

namespace AzureMobileClient.Helpers.Accounts
{
    public static class JsonSerializer
	{
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