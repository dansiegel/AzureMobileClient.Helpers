using SimpleInjector;
using System;
using System.Reflection;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides registration extensions for SimpleInjector
    /// </summary>
    public static class SimpleInjectorRegistrationExtensions
    {
        /// <summary>
        /// Registers the default <see cref="AzureCloudSyncTable{T}" /> implementation for sync tables
        /// </summary>
        public static Container RegisterSyncTableProvider(this Container container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudSyncTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudSyncTable{T}" />
        /// </summary>
        public static Container RegisterSyncTableProvider(this Container container, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("The specified type must be a Generic type");
            }

            if (type.GetTypeInfo().GetGenericTypeDefinition() != typeof(ICloudSyncTable<>))
            {
                throw new ArgumentException("The specified type must be of type 'ICloudSyncTable<>'");
            }

            container.Register(typeof(ICloudSyncTable<>), type);
            return container;
        }

        /// <summary>
        /// Registers the default <see cref="AzureCloudTable{T}" /> table provider
        /// </summary>
        public static Container RegisterTableProvider(this Container container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudTable{T}" />
        /// </summary>
        public static Container RegisterTableProvider(this Container container, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("The specified type must be a Generic type");
            }

            if (type.GetTypeInfo().GetGenericTypeDefinition() != typeof(ICloudTable<>))
            {
                throw new ArgumentException("The specified type must be of type 'ICloudTable<>'");
            }

            container.Register(typeof(ICloudTable<>), type);
            return container;
        }
    }
}