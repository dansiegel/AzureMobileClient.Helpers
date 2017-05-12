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

        /// <summary>
        /// Gets the Offline Database Path/Database name
        /// </summary>
        protected string OfflineDbPath { get; }

        /// <summary>
        /// Gets the Local Store. Uses the MobileServiceSQLiteStore by default
        /// </summary>
        protected virtual MobileServiceLocalStore GetLocalStore() =>
            new MobileServiceSQLiteStore(OfflineDbPath);

        /// <summary>
        /// Initializes the Context
        /// </summary>
        protected virtual void Initialize()
        {
            var store = GetLocalStore();
            LocalStoreConfiguration.DefineTables(store, GetType());
            Client.SyncContext.InitializeAsync(store);
        }

        public abstract ICloudSyncTable<T> SyncTable<T>() where T : EntityData;

        public abstract ICloudTable<T> Table<T>() where T : EntityData;
    }
}