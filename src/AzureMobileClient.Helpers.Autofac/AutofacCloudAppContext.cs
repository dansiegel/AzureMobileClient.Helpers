using Autofac;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a base implementation for <see cref="ICloudAppContext" />
    /// </summary>
    public abstract class AutofacCloudAppContext : AzureCloudAppContext
    {
        /// <summary>
        /// Gets the resolving context.
        /// </summary>
        protected IComponentContext Context { get; }

        /// <summary>
        /// Constructs a new <see cref="AutofacCloudAppContext" />
        /// </summary>
        public AutofacCloudAppContext(IComponentContext context, string offlineDbPath = _offlineDbPath)
            : base(context.Resolve<IMobileServiceClient>(), offlineDbPath)
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