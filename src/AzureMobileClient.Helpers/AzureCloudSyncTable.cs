using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// AzureCloudSyncTable
    /// </summary>
    public class AzureCloudSyncTable<T> : ICloudSyncTable<T> where T : TableData
    {
        private IMobileServiceClient _client { get; }
        private IMobileServiceSyncTable<T> table { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AzureCloudSyncTable(IMobileServiceClient)" />
        /// </summary>
        public AzureCloudSyncTable(IMobileServiceClient client)
        {
            _client = client;
            table = _client.GetSyncTable<T>();
        }

        #region ICloudSyncTable interface
        /// <inheritDoc />
        public virtual async Task PullAsync()
        {
            string queryName = $"incsync_{typeof(T).Name}";
            await table.PullAsync(queryName, table.CreateQuery());
        }

        /// <inheritDoc />
        public virtual async Task<T> CreateItemAsync(T item)
        {
            await table.InsertAsync(item);
            return item;
        }

        /// <inheritDoc />
        public virtual async Task DeleteItemAsync(T item)
            => await table.DeleteAsync(item);

        /// <inheritDoc />
        public virtual async Task<ICollection<T>> ReadAllItemsAsync()
        {
            return await table.ToListAsync();
        }

        /// <inheritDoc />
        public virtual async Task<T> ReadItemAsync(string id)
            => await table.LookupAsync(id);

        /// <inheritDoc />
        public virtual async Task<ICollection<T>> ReadItemsAsync(int start, int count)
        {
            return await table.Skip(start).Take(count).ToListAsync();
        }

        /// <inheritDoc />
        public virtual async Task<T> UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
            return item;
        }
        #endregion
    }
}