using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Foundation;
using Security;

namespace AzureMobileClient.Helpers.Accounts
{
    public class SecureStore : ISecureStore
    {
        const string FILENAME = "AzureMobileClient.Helpers.data";
        const string PREFERENCES_KEY = "Settings";

        static readonly object lockStore = new object();

        Dictionary<string, string> store = null;

        public string this[string key]
        {
            get
            {
                lock (lockStore)
                {
                    lock (lockStore)
                    {
                        if (store == null)
                            store = Load() ?? new Dictionary<string, string>();

                        if (!store.ContainsKey(key))
                            return null;

                        return store?[key];
                    }
                }
            }
            set
            {
                lock (lockStore)
                {
                    if (store == null)
                        store = Load() ?? new Dictionary<string, string>();

                    if (store.ContainsKey(key))
                        store[key] = value;
                    else
                        store.Add(key, value);

                    Save(store);
                }
            }
        }

        Dictionary<string, string> Load()
        {
            //var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            try
            {
                var kc = new KeyChain();
                var serialized = kc.ValueForKey(PREFERENCES_KEY);

                return JsonSerializer.Deserialize<Dictionary<string, string>>(serialized);

            }
            catch
            {
            }

            return new Dictionary<string, string>();
            //var bytes = Encoding.UTF8.GetBytes(serialized);

            //using (var ms = new MemoryStream(bytes))
            //return (Dictionary<string, string>)serializer.ReadObject(ms);
        }

        void Save(Dictionary<string, string> data)
        {
            var serialized = JsonSerializer.Serialize(data);
            //var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));

            //var serialized = string.Empty;

            //using (var ms = new MemoryStream())
            //{
            //	serializer.WriteObject(ms, data);
            //	serialized = Encoding.Default.GetString(ms.ToArray());
            //}

            var kc = new KeyChain();
            kc.SetValueForKey(serialized, PREFERENCES_KEY);
        }

        class KeyChain
        {
            public string ValueForKey(string key)
            {
                var record = ExistingRecordForKey(key);
                SecStatusCode resultCode;
                var match = SecKeyChain.QueryAsRecord(record, out resultCode);

                if (resultCode == SecStatusCode.Success)
                    return NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
                else
                    return String.Empty;
            }

            public void SetValueForKey(string value, string key)
            {
                var record = ExistingRecordForKey(key);
                if (string.IsNullOrEmpty(value))
                {
                    if (!string.IsNullOrEmpty(ValueForKey(key)))
                        RemoveRecord(record);

                    return;
                }

                // if the key already exists, remove it
                if (!string.IsNullOrEmpty(ValueForKey(key)))
                    RemoveRecord(record);

                var result = SecKeyChain.Add(CreateRecordForNewKeyValue(key, value));
                if (result != SecStatusCode.Success)
                {
                    throw new Exception(String.Format("Error adding record: {0}", result));
                }
            }

            private SecRecord CreateRecordForNewKeyValue(string key, string value)
            {
                return new SecRecord(SecKind.GenericPassword)
                {
                    Account = key,
                    Service = FILENAME,
                    Label = key,
                    ValueData = NSData.FromString(value, NSStringEncoding.UTF8),
                };
            }

            private SecRecord ExistingRecordForKey(string key)
            {
                return new SecRecord(SecKind.GenericPassword)
                {
                    Account = key,
                    Service = FILENAME,
                    Label = key,
                };
            }

            private bool RemoveRecord(SecRecord record)
            {
                var result = SecKeyChain.Remove(record);
                if (result != SecStatusCode.Success)
                {
                    throw new Exception(String.Format("Error removing record: {0}", result));
                }

                return true;
            }
        }
    }
}