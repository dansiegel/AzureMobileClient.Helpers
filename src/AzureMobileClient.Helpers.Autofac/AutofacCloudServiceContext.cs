using Autofac;
using Microsoft.WindowsAzure.MobileServices; 

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudService" /> and <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class DryIocCloudServiceContext : AzureCloudServiceContext
    {
        /// <summary>
        /// Gets the Resolving Context
        /// </summary>
        protected IComponentContext Context { get; } 

        /// <summary>
        /// Constructs a new <see cref="DryIocCloudServiceContext" />
        /// </summary>
        public DryIocCloudServiceContext(IComponentContext context, IAzureCloudServiceOptions options, ILoginProvider loginProvider, string offlineDbPath = _offlineDbPath)
            : base(options, loginProvider, offlineDbPath)
        {
            Context = context;
        }

        /// <inheritDoc />
        public override ICloudSyncTable<T> SyncTable<T>() =>
            Context.Resolve<ICloudSyncTable<T>>();

        /// <inheritDoc />
        public override ICloudTable<T> Table<T>() =>
            Context.Resolve<ICloudTable<T>>();
    }
}