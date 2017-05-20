using DryIoc;
using System;
using System.Reflection;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides registration extensions for DryIoc
    /// <summary>
    public static class DryIocRegistrationExtensions
    {
        /// <summary>
        /// Registers the default <see cref="AzureCloudSyncTable<>" /> implementation for sync tables
        /// </summary>
        public static IContainer RegisterSyncTableProvider(this IContainer container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudSyncTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudSyncTable<>" />
        /// <summary>
        public static IContainer RegisterSyncTableProvider(this IContainer container, Type type)
        {
            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if(!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("The specified type must be a Generic type");
            }

            if(type.GetTypeInfo().GetGenericTypeDefinition() != typeof(ICloudSyncTable<>))
            {
                throw new ArgumentException("The specified type must be of type 'ICloudSyncTable<>'");
            }

            container.Register(typeof(ICloudSyncTable<>), type);
            return container;
        }

        /// <summary>
        /// Registers the default <see cref="AzureCloudTable<>" /> table provider
        /// </summary>
        public static IContainer RegisterTableProvider(this IContainer container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudTable<>" />
        /// </summary>
        public static IContainer RegisterTableProvider(this IContainer container, Type type)
        {
            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if(!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("The specified type must be a Generic type");
            }

            if(type.GetTypeInfo().GetGenericTypeDefinition() != typeof(ICloudTable<>))
            {
                throw new ArgumentException("The specified type must be of type 'ICloudTable<>'");
            }

            container.Register(typeof(ICloudTable<>), type);
            return container;
        }
    }
}