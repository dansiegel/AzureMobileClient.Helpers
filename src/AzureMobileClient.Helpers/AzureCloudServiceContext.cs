using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace AzureMobileClient.Helpers
{
    public abstract class AzureCloudServiceContext : AzureCloudService
    {
        /// <summary>
        /// Default App Context database name
        /// </summary>
        protected const string _offlineDbPath = "azureCloudAppContext.db";

        public AzureCloudServiceContext(IAzureCloudServiceOptions options, ILoginProvider loginProvider, string offlineDbPath = _offlineDbPath) 
            : base(options, loginProvider)
        {
            OfflineDbPath = offlineDbPath;
            Initialize();
        }

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

        public abstract ICloudSyncTable<T> SyncTable<T>() where T : TableData;

        public abstract ICloudTable<T> Table<T>() where T : TableData;
    }
}