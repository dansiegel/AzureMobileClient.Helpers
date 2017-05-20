using DryIoc;
using Microsoft.WindowsAzure.MobileServices; 

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudService" /> and <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class DryIocCloudServiceContext : AzureCloudServiceContext
    {
        /// <summary>
        /// Gets the Resolving Container
        /// </summary>
        protected IContainer Container { get; } 

        /// <summary>
        /// Constructs a new <see cref="DryIocCloudServiceContext" />
        /// </summary>
        public DryIocCloudServiceContext(IContainer container, IAzureCloudServiceOptions options, ILoginProvider loginProvider, string offlineDbPath = _offlineDbPath)
            : base(options, loginProvider, offlineDbPath)
        {
            Container = container;
        }

        /// <inheritDoc />
        public override ICloudSyncTable<T> SyncTable<T>() =>
            Container.Resolve<ICloudSyncTable<T>>();

        /// <inheritDoc />
        public override ICloudTable<T> Table<T>() =>
            Container.Resolve<ICloudTable<T>>();
    }
}