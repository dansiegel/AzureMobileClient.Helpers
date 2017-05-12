using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// ICloudSyncTable
    /// </summary>
    public interface ICloudSyncTable<T> : ICloudTable<T> where T : EntityData
    {
        /// <summary>
        /// Pulls the latest data from the server and ensures proper syncing
        /// </summary>
        Task PullAsync();

        /// <summary>
        /// Gets the count of any pending operations
        /// </summary>
        long PendingOperations { get; }

        /// <summary>
        /// Synchronize the table with the cloud store
        /// </summary>
        Task Sync(CancellationToken cancellationToken = default(CancellationToken));
    }
}