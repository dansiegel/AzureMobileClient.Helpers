using SimpleInjector;
using Microsoft.WindowsAzure.MobileServices;
using AzureMobileClient.Helpers.Accounts;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudService{TAccount}" /> and <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class SimpleInjectorCloudServiceContext<TAccount> : AzureCloudServiceContext<TAccount>
        where TAccount : IAccount
    {
        /// <summary>
        /// Gets the Resolving Container
        /// </summary>
        protected Container Container { get; }

        /// <summary>
        /// Constructs a new <see cref="SimpleInjectorCloudServiceContext{TAccount}" />
        /// </summary>
        public SimpleInjectorCloudServiceContext(Container container, IAzureCloudServiceOptions options, ILoginProvider<TAccount> loginProvider, string offlineDbPath = _offlineDbPath)
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