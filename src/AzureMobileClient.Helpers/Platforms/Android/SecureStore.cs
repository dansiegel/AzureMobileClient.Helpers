
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using Android.App;
using Android.Content;
using Android.OS;
using Java.Security;
using Javax.Crypto;
using AzureMobileClient.Helpers.Platforms.Android;

namespace AzureMobileClient.Helpers.Accounts
{
    public class SecureStore : ISecureStore
    {
        const string FILENAME = "AzureMobileClient.Helpers.data";
        const string PREFERENCES_KEY = "Settings";

        static readonly object lockStore = new object();

        ActivityLifecycleCallbackManager activityLifecycleManager { get; }

        Dictionary<string, string> store = null;

        public SecureStore(Android.App.Application app)
        {
            activityLifecycleManager = new ActivityLifecycleCallbackManager();
            app.RegisterActivityLifecycleCallbacks(activityLifecycleManager);
        }

        public Android.Support.V4.App.FragmentActivity CurrentActivity => 
            activityLifecycleManager?.CurrentActivity;


        public string this[string key]
        {
            get
            {
                lock (lockStore)
                {
                    if (store == null)
                    {
                        store = Load(CurrentActivity) ?? new Dictionary<string, string>();
                    }

                    if (!store.ContainsKey(key))
                        return null;

                    return store?[key];
                }
            }
            set
            {
                lock (lockStore)
                {
                    if (store == null)
                        store = Load(CurrentActivity) ?? new Dictionary<string, string>();

                    if (store.ContainsKey(key))
                        store[key] = value;
                    else
                        store.Add(key, value);

                    Save(CurrentActivity, store);
                }
            }
        }

        Dictionary<string, string> Load(Context context)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));

                var keyStore = LoadKeyStore(context);

                var entry = keyStore.Item1.GetEntry(FILENAME, keyStore.Item2) as KeyStore.SecretKeyEntry;

                if (entry != null)
                {
                    var bytes = entry.SecretKey.GetEncoded();

                    using (var ms = new MemoryStream(bytes))
                        return (Dictionary<string, string>)serializer.ReadObject(ms);
                }
            }
            catch { }

            return new Dictionary<string, string>();
        }

        void Save(Context context, Dictionary<string, string> data)
        {
            var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));

            var serialized = string.Empty;

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                serialized = System.Text.Encoding.Default.GetString(ms.ToArray());
            }

            var ks = LoadKeyStore(context);
            ks.Item1.SetEntry(FILENAME, new KeyStore.SecretKeyEntry(new SecretEntry(serialized)), ks.Item2);
            SaveKeyStore(context, ks.Item1);
        }


        static readonly object fileLock = new object();
        static Tuple<KeyStore, KeyStore.PasswordProtection> LoadKeyStore(Context context)
        {
            // Get our secure key which will be randomly created the first time the app is run
            var secureKey = GetSecureKey(context);
            var keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
            var prot = new KeyStore.PasswordProtection(secureKey);

            try
            {
                lock (fileLock)
                {
                    if (context.GetFileStreamPath(FILENAME)?.Exists() ?? false)
                    {
                        using (var s = context.OpenFileInput(FILENAME))
                            keyStore.Load(s, secureKey);
                    }
                    else
                    {
                        keyStore.Load(null, secureKey);
                    }
                }
            }
            catch
            {
                keyStore.Load(null, secureKey);
            }

            return Tuple.Create(keyStore, prot);
        }

        static void SaveKeyStore(Context context, KeyStore keyStore)
        {
            lock (fileLock)
            {
                using (var s = context.OpenFileOutput(FILENAME, FileCreationMode.Private))
                {
                    keyStore.Store(s, GetSecureKey(context));
                    s.Flush();
                    s.Close();
                }
            }

        }

        // The secure key will be created if it doesn't exist the first time it's needed
        // We then store the key in the private shared preferences so only this app has access
        // to it.
        static char[] GetSecureKey(Context context)
        {
            const string CACHEKEY_KEY = "CacheKey";

            var cacheKey = string.Empty;
            var prefs = context.GetSharedPreferences(PREFERENCES_KEY, FileCreationMode.Private);

            if (prefs.Contains(CACHEKEY_KEY))
            {
                cacheKey = prefs.GetString(CACHEKEY_KEY, string.Empty);

                if (!string.IsNullOrEmpty(cacheKey))
                    return cacheKey.ToCharArray();
            }

            // Generate a 256-bit key
            const int outputKeyLength = 256;

            var secureRandom = new SecureRandom();
            // Do *not* seed secureRandom! Automatically seeded from system entropy.
            var keyGenerator = KeyGenerator.GetInstance("AES");
            keyGenerator.Init(outputKeyLength, secureRandom);
            var key = keyGenerator.GenerateKey();
            cacheKey = Convert.ToBase64String(key.GetEncoded());

            prefs.Edit()
                    .PutString(CACHEKEY_KEY, cacheKey)
                    .Commit();

            return cacheKey.ToCharArray();
        }

        class SecretEntry : Java.Lang.Object, ISecretKey
        {
            byte[] bytes;
            public SecretEntry(string value)
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(value);
            }
            public string Algorithm { get { return "RAW"; } }

            public string Format { get { return "RAW"; } }

            public byte[] GetEncoded()
            {
                return bytes;
            }
        }
    }
}