using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// An ICloudTable of a TableData type
    /// </summary>
    public interface ICloudTable<T> where T : EntityData
    {
        /// <summary>
        /// Creates a new Item asynchronously 
        /// </summary>
        Task<T> CreateItemAsync(T item);

        /// <summary>
        /// Gets an item asynchronously from a specified id
        /// </summary>
        Task<T> ReadItemAsync(string id);

        /// <summary>
        /// Updates an Item asynchronously
        /// </summary>
        Task<T> UpdateItemAsync(T item);

        /// <summary>
        /// Deletes an Item asynchronously
        /// </summary>
        Task DeleteItemAsync(T item);

        /// <summary>
        /// Retrieves all of the table items asynchronously
        /// </summary>
        Task<ICollection<T>> ReadAllItemsAsync();

        /// <summary>
        /// Retrieves the items within a specified range asynchronously
        /// </summary>
        Task<ICollection<T>> ReadItemsAsync(int start, int count);
    }
}