using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Base TableData object
    /// </summary>
    public abstract class TableData : INotifyPropertyChanged
    {
        private string _id;
        private DateTimeOffset? _updatedAt;
        private DateTimeOffset? _createdAt;
        private byte[] _version;
        private bool _deleted;

        /// <summary>
        /// Object Id
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        /// <summary>
        /// When the object was last updated
        /// </summary>
        [UpdatedAt]
        public DateTimeOffset? UpdatedAt
        {
            get { return _updatedAt; }
            set { SetProperty(ref _updatedAt, value); }
        }

        /// <summary>
        /// When the object was created
        /// </summary>
        [CreatedAt]
        public DateTimeOffset? CreatedAt
        {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }

        /// <summary>
        /// The Azure Mobile Service object Version
        /// </summary>
        [Version]
        public byte[] Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        [Deleted]
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
    }
}
