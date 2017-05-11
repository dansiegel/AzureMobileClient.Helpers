using DryIoc;
using Microsoft.WindowsAzure.MobileServices; 

namespace AzureMobileClient.Helpers
{
    public class DryIocCloudServiceContext : AzureCloudServiceContext
    {
        protected IContainer Container { get; } 

        public DryIocCloudServiceContext(IContainer container, IAzureCloudServiceOptions options, ILoginProvider loginProvider, string offlineDbPath = _offlineDbPath)
            : base(options, loginProvider, offlineDbPath)
        {
            Container = container;
        }

        public override ICloudSyncTable<T> SyncTable<T>() =>
            Container.Resolve<ICloudSyncTable<T>>();

        public override ICloudTable<T> Table<T>() =>
            Container.Resolve<ICloudTable<T>>();
    }
}