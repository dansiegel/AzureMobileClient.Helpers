using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// An ICloudTable of a TableData type
    /// </summary>
    public interface ICloudTable<T> where T : IEntityData
    {
        /// <summary>
        /// Creates a new Item asynchronously 
        /// </summary>
        Task<T> CreateItemAsync(T item);

        /// <summary>
        /// Gets an item asynchronously from a specified id
        /// </summary>
        Task<T> ReadItemAsync(string id);

        Task<T> ReadItemAsync(Expression<System.Func<T, bool>> predicate);

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

        Task<ICollection<T>> ReadItemsAsync(Expression<System.Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves the items within a specified range asynchronously
        /// </summary>
        Task<ICollection<T>> ReadItemsAsync(int start, int count);
    }
}