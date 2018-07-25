using AzureMobileClient.Helpers.Accounts;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudService{TAccount}" /> and <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class AzureCloudServiceContext<TAccount> : AzureCloudService<TAccount>, ICloudAppContext
        where TAccount : IAccount
    {
        /// <summary>
        /// Default App Context database name
        /// </summary>
        protected const string _offlineDbPath = "azureCloudAppContext.db";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCloudServiceContext{TAccount}"/>
        /// </summary>
        public AzureCloudServiceContext(IAzureCloudServiceOptions options, ILoginProvider<TAccount> loginProvider, string offlineDbPath = _offlineDbPath) 
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

        /// <inheritDoc />
        public abstract ICloudSyncTable<T> SyncTable<T>() where T : IEntityData;

        /// <inheritDoc />
        public abstract ICloudTable<T> Table<T>() where T : IEntityData;
    }
}