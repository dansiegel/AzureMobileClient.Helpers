using DryIoc;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    public abstract class DryIocCloudAppContext : AzureCloudAppContext
    {
        protected IContainer Container { get; }

        public DryIocCloudAppContext(IContainer container, string offlineDbPath = _offlineDbPath)
            : base(container.Resolve<IMobileServiceClient>(), offlineDbPath)
        {
            Container = container;
        }

        public override ICloudSyncTable<T> SyncTable<T>() =>
            Container.Resolve<ICloudSyncTable<T>>();

        public override ICloudTable<T> Table<T>() =>
            Container.Resolve<ICloudTable<T>>();
    }
}