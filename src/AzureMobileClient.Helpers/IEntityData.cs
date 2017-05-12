using System;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Azure Entity Data Model
    /// </summary>
    public interface IEntityData
    {
        /// <summary>
        /// Object Id
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// When the object was last updated
        /// </summary>
        DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// When the object was created
        /// </summary>
        DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The Azure Mobile Service object Version
        /// </summary>
        byte[] Version { get; set; }

        /// <summary>
        /// Indicates whether the object was deleted
        /// </summary>
        bool Deleted { get; set; }
    }
}