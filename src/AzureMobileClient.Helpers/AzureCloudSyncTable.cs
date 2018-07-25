using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// AzureCloudSyncTable
    /// </summary>
    public class AzureCloudSyncTable<T> : ICloudSyncTable<T> where T : EntityData
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
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityChanged;
        }

        /// <summary>
        /// Gets the pending operation count
        /// </summary>
        public long PendingOperations { get { return _client.SyncContext.PendingOperations; } }

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
            if(_client.SyncContext.PendingOperations > 0 && CrossConnectivity.Current.IsConnected)
            {
                await _client.SyncContext.PushAsync();
            }
            return item;
        }

        /// <inheritDoc />
        public virtual async Task DeleteItemAsync(T item)
            => await table.DeleteAsync(item);

        /// <inheritDoc />
        public virtual async Task<ICollection<T>> ReadAllItemsAsync() =>
            await table.ToListAsync();

        /// <inheritDoc />
        public virtual async Task<T> ReadItemAsync(string id)
            => await table.LookupAsync(id);

        /// <inheritDoc />
        public virtual async Task<T> ReadItemAsync(Expression<System.Func<T, bool>> predicate) =>
            (await table.Where(predicate).Take(1).ToListAsync()).FirstOrDefault();

        /// <inheritDoc />
        public virtual async Task<ICollection<T>> ReadItemsAsync(int start, int count)
        {
            return await table.Skip(start).Take(count).ToListAsync();
        }

        /// <inheritDoc />
        public virtual async Task<ICollection<T>> ReadItemsAsync(Expression<System.Func<T, bool>> predicate) =>
            await table.Where(predicate).ToListAsync();

        /// <inheritDoc />
        public virtual async Task<T> UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
            return item;
        }

        private DateTimeOffset? LastSync;

        /// <inheritDoc />
        public virtual async Task SyncAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if(!CrossConnectivity.Current.IsConnected) return;

            if(PendingOperations > 0)
            {
                await _client.SyncContext.PushAsync(cancellationToken);
            }

            await PullAsync();

            LastSync = DateTimeOffset.Now;
        }
        #endregion

        private async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            await OnConnectivityChangedAsync(e);
        }

        /// <summary>
        /// Handles Activity Connection Changes. 
        /// </summary>
        protected virtual async Task OnConnectivityChangedAsync(ConnectivityChangedEventArgs e)
        {
            if(e.IsConnected && (LastSync == null || DateTimeOffset.Now - LastSync.Value > TimeSpan.FromSeconds(10)))
            {
                await SyncAsync();
            }
        }
    }
}