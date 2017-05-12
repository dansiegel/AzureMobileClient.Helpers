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

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCloudServiceContext(IAzureCloudServiceOptions, ILoginProvider, string)" />
        /// </summary>
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
        /// Indicates if the Context has Sync Tables
        /// </summary>
        protected bool HasSyncTables { get; private set; }

        /// <summary>
        /// Initializes the Context
        /// </summary>
        protected virtual void Initialize()
        {
            var store = GetLocalStore();
            if(HasSyncTables = LocalStoreConfiguration.DefineTables(store, GetType()))
            {
                Client.SyncContext.InitializeAsync(store);
            }
        }

        /// <summary>
        /// Gets the SyncTable
        /// </summary>
        public abstract ICloudSyncTable<T> SyncTable<T>() where T : EntityData;

        /// <summary>
        /// Gets the Table
        /// </summary>
        public abstract ICloudTable<T> Table<T>() where T : EntityData;
    }
}