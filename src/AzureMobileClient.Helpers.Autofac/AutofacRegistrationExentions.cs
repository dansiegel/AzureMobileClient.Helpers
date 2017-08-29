using Autofac;
using System;
using System.Reflection;

namespace AzureMobileClient.Helpers
{
    /// <summary>
    /// Provides registration extensions for Autofac
    /// <summary>
    public static class AutofacRegistrationExtensions
    {
        /// <summary>
        /// Registers the default <see cref="AzureCloudSyncTable<>" /> implementation for sync tables
        /// </summary>
        public static ContainerBuilder RegisterSyncTableProvider(this ContainerBuilder builder) =>
            builder.RegisterSyncTableProvider(typeof(AzureCloudSyncTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudSyncTable<>" />
        /// <summary>
        public static ContainerBuilder RegisterSyncTableProvider(this ContainerBuilder builder, Type type)
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

            builder.RegisterType(type).As(typeof(ICloudSyncTable<>)).SingleInstance();

            return builder;
        }

        /// <summary>
        /// Registers the default <see cref="AzureCloudTable<>" /> table provider
        /// </summary>
        public static ContainerBuilder RegisterTableProvider(this ContainerBuilder builder) =>
            builder.RegisterSyncTableProvider(typeof(AzureCloudTable<>));

        /// <summary>
        /// Registers your custom implementation of <see cref="ICloudTable<>" />
        /// </summary>
        public static ContainerBuilder RegisterTableProvider(this ContainerBuilder builder, Type type)
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

            builder.RegisterType(type).As(typeof(ICloudTable<>)).SingleInstance();

            return builder;
        }
    }
}