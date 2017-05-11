using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// AzureCloudAppContext provides a base context for accessing your <see cref="ICloudTable" /> and
    /// <see cref="ICloudSyncTable" />
    /// </summary>
    public abstract class AzureCloudAppContext
    {
        /// <summary>
        /// Default App Context database name
        /// </summary>
        protected const string _offlineDbPath = "azureCloudAppContext.db";

        /// <summary>
        /// Initializes a new instance of <see cref="AzureCloudAppContext(IMobileServiceClient,string)" />
        /// </summary>
        public AzureCloudAppContext(IMobileServiceClient client, string offlineDbPath = _offlineDbPath)
        {
            Client = client;
            OfflineDbPath = offlineDbPath;
            Initialize();
        }

        /// <summary>
        /// Gets the underlying IMobileServiceClient
        /// </summary>
        protected IMobileServiceClient Client { get; }

        protected string OfflineDbPath { get; private set; }

        protected virtual MobileServiceLocalStore GetLocalStore() =>
            new MobileServiceSQLiteStore(OfflineDbPath);

        protected virtual void Initialize()
        {
            var store = GetLocalStore();
            DefineTables(store);
            Client.SyncContext.InitializeAsync(store);
        }

        protected virtual void DefineTables(MobileServiceLocalStore store)
        {
            foreach(var pi in GetType().GetTypeInfo().DeclaredProperties)
            {
                Type type = pi.PropertyType;
                if(type.GetTypeInfo().IsGenericType && 
                    (type.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICloudSyncTable<>) || 
                     type.GetTypeInfo().GetGenericTypeDefinition() == typeof(IMobileServiceSyncTable<>)))
                {
                    DefineTable(store, type.GetTypeInfo().GenericTypeArguments.FirstOrDefault());
                }
            }
        }

        public abstract ICloudSyncTable<T> SyncTable<T>() where T : TableData;

        public abstract ICloudTable<T> Table<T>() where T : TableData;

        private void DefineTable(MobileServiceLocalStore store, Type type)
        {
            if(type == null)
            {
                throw new ArgumentNullException("The data table type cannot be null");
            }

            // Adopted from SQLite Store Generic Extensions
            var settings = new MobileServiceJsonSerializerSettings();
            var contract = settings.ContractResolver.ResolveContract(type) as JsonObjectContract;
            if (contract.DefaultCreator == null)
            {
                throw new ArgumentException($"The TableData type '{type.Name}' does not have parameterless constructor.");
            }

            object theObject = contract.DefaultCreator();
            SetEnumDefault(contract, theObject);
            var item = ConvertToJObject(settings, theObject);

            //// set default values so serialized version can be used to infer types
            SetIdDefault(settings, type, item);
            SetNullDefault(contract, item);

            store.DefineTable(type.Name, item);
        }

        private static void SetEnumDefault(JsonObjectContract contract, object theObject)
        {
            foreach (JsonProperty contractProperty in contract.Properties)
            {
                if (contractProperty.PropertyType.GetTypeInfo().IsEnum)
                {
                    object firstValue = Enum.GetValues(contractProperty.PropertyType)
                                            .Cast<object>()
                                            .FirstOrDefault();
                    if (firstValue != null)
                    {
                        contractProperty.ValueProvider.SetValue(theObject, firstValue);
                    }
                }
            }
        }

        private static JObject ConvertToJObject(MobileServiceJsonSerializerSettings settings, object theObject)
        {
            string json = JsonConvert.SerializeObject(theObject, settings);
            JObject item = JsonConvert.DeserializeObject<JObject>(json, settings);
            return item;
        }

        private static void SetIdDefault(MobileServiceJsonSerializerSettings settings, Type dataType, JObject item)
        {
            JsonProperty idProperty = settings.ContractResolver.ResolveIdProperty(dataType);
            if (idProperty.PropertyType == typeof(long) || idProperty.PropertyType == typeof(int))
            {
                item[MobileServiceSystemColumns.Id] = 0;
            }
            else
            {
                item[MobileServiceSystemColumns.Id] = String.Empty;
            }
        }

        private static void SetNullDefault(JsonObjectContract contract, JObject item)
        {
            foreach (JProperty itemProperty in item.Properties().Where(i => i.Value.Type == JTokenType.Null))
            {
                JsonProperty contractProperty = contract.Properties[itemProperty.Name];
                if (contractProperty.PropertyType == typeof(string) || contractProperty.PropertyType == typeof(Uri))
                {
                    item[itemProperty.Name] = String.Empty;
                }
                else if (contractProperty.PropertyType == typeof(byte[]))
                {
                    item[itemProperty.Name] = new byte[0];
                }
                else if (contractProperty.PropertyType.GetTypeInfo().IsGenericType && contractProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    item[itemProperty.Name] = new JValue(Activator.CreateInstance(contractProperty.PropertyType.GenericTypeArguments[0]));
                }
                else
                {
                    item[itemProperty.Name] = new JObject();
                }
            }
        }
    }
}