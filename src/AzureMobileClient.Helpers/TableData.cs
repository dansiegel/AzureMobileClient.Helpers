using System;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Base TableData object
    /// </summary>
    public abstract class TableData
    {
        /// <summary>
        /// Object Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// When the object was last updated
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// When the object was created
        /// </summary>
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The Azure Mobile Service object Version
        /// </summary>
        public byte[] Version { get; set; }
    }
}
