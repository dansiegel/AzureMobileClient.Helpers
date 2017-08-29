using SimpleInjector;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudService" /> and <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class SimpleInjectorCloudServiceContext : AzureCloudServiceContext
    {
        /// <summary>
        /// Gets the Resolving Container
        /// </summary>
        protected Container Container { get; }

        /// <summary>
        /// Constructs a new <see cref="SimpleInjectorCloudServiceContext" />
        /// </summary>
        public SimpleInjectorCloudServiceContext(Container container, IAzureCloudServiceOptions options, ILoginProvider loginProvider, string offlineDbPath = _offlineDbPath)
            : base(options, loginProvider, offlineDbPath)
        {
            Container = container;
        }

        /// <inheritDoc />
        public override ICloudSyncTable<T> SyncTable<T>() =>
            Container.GetInstance<ICloudSyncTable<T>>();

        /// <inheritDoc />
        public override ICloudTable<T> Table<T>() =>
            Container.GetInstance<ICloudTable<T>>();
    }
}