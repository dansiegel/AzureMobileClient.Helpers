using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudService" /> and <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class UnityCloudServiceContext : AzureCloudServiceContext
    {
        /// <summary>
        /// Gets the Resolving Container
        /// </summary>
        protected IUnityContainer Container { get; }

        /// <summary>
        /// Constructs a new <see cref="UnityCloudServiceContext" />
        /// </summary>
        public UnityCloudServiceContext(IUnityContainer container, IAzureCloudServiceOptions options, ILoginProvider loginProvider, string offlineDbPath = _offlineDbPath)
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