using DryIoc;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class DryIocCloudAppContext : AzureCloudAppContext
    {
        /// <summary>
        /// Gets the resolving container
        /// </summary>
        protected IContainer Container { get; }

        /// <summary>
        /// Constructs a new <see cref="DryIocCloudAppContext" />
        /// </summary>
        public DryIocCloudAppContext(IContainer container, string offlineDbPath = _offlineDbPath)
            : base(container.Resolve<IMobileServiceClient>(), offlineDbPath)
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