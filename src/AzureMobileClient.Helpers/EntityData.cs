using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Base TableData object
    /// </summary>
    public abstract class EntityData : IEntityData, INotifyPropertyChanged
    {
        private string _id;
        private DateTimeOffset? _updatedAt;
        private DateTimeOffset? _createdAt;
        private byte[] _version;
        private bool _deleted;

        /// <inheritDoc />
        [JsonProperty("id")]
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        /// <inheritDoc />
        [UpdatedAt]
        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt
        {
            get { return _updatedAt; }
            set { SetProperty(ref _updatedAt, value); }
        }

        /// <inheritDoc />
        [CreatedAt]
        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt
        {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }

        /// <inheritDoc />
        [Version]
        [JsonProperty("version")]
        public byte[] Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        /// <inheritDoc />
        [Deleted]
        [JsonProperty("deleted")]
        public bool Deleted
        {
            get { return _deleted; }
            set { SetProperty(ref _deleted, value); }
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
        /// <param name="backingStore">Backing store.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">On changed.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected virtual bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <inheritDoc />
        public override bool Equals(object obj)
        {
            var entityB = obj as IEntityData;
            return string.Equals(Id?.Trim(), entityB.Id?.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritDoc />
        public override int GetHashCode() => Id.ToLower().GetHashCode();
    }
}
