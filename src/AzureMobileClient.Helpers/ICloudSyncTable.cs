using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// ICloudSyncTable
    /// </summary>
    public interface ICloudSyncTable<T> : ICloudTable<T> where T : TableData
    {
        /// <summary>
        /// Pulls the latest data from the server and ensures proper syncing
        /// </summary>
        Task PullAsync();
    }
}