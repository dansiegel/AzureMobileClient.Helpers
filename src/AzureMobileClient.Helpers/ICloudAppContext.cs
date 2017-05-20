namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides a definition for how a Cloud App Context can get a Cloud Table or Cloud Sync Table
    /// </summary>
    public interface ICloudAppContext
    {
        /// <summary>
        /// Gets the SyncTable
        /// </summary>
        ICloudSyncTable<TEntity> SyncTable<TEntity>() where TEntity : IEntityData;

        /// <summary>
        /// Gets the Table
        /// </summary>
        ICloudTable<TEntity> Table<TEntity>() where TEntity : IEntityData;
    }
}