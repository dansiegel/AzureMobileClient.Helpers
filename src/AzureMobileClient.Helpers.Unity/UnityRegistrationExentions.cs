using Unity;
using System;
using System.Reflection;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides registration extensions for Unity
    /// </summary>
    public static class UnityRegistrationExtensions
    {
        /// <summary>
        /// Registers the default <see cref="AzureCloudSyncTable{T}" /> implementation for sync tables
        /// </summary>
        public static IUnityContainer RegisterSyncTableProvider(this IUnityContainer container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudSyncTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudSyncTable{T}" />
        /// </summary>
        public static IUnityContainer RegisterSyncTableProvider(this IUnityContainer container, Type type)
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

            container.RegisterType(typeof(ICloudSyncTable<>), type);
            return container;
        }

        /// <summary>
        /// Registers the default <see cref="AzureCloudTable{T}" /> table provider
        /// </summary>
        public static IUnityContainer RegisterTableProvider(this IUnityContainer container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudTable{T}" />
        /// </summary>
        public static IUnityContainer RegisterTableProvider(this IUnityContainer container, Type type)
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

            container.RegisterType(typeof(ICloudTable<>), type);
            return container;
        }
    }
}