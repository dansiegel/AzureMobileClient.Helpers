using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class UnityCloudAppContext : AzureCloudAppContext
    {
        /// <summary>
        /// Gets the resolving container
        /// </summary>
        protected IUnityContainer Container { get; }

        /// <summary>
        /// Constructs a new <see cref="UnityCloudAppContext" />
        /// </summary>
        public UnityCloudAppContext(IUnityContainer container, string offlineDbPath = _offlineDbPath)
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