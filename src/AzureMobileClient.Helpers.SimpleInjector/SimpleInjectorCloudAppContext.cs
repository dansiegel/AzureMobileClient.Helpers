using SimpleInjector;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class SimpleInjectorCloudAppContext : AzureCloudAppContext
    {
        /// <summary>
        /// Gets the resolving container
        /// </summary>
        protected Container Container { get; }

        /// <summary>
        /// Constructs a new <see cref="SimpleInjectorCloudAppContext" />
        /// </summary>
        public SimpleInjectorCloudAppContext(Container container, string offlineDbPath = _offlineDbPath)
            : base(container.GetInstance<IMobileServiceClient>(), offlineDbPath)
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