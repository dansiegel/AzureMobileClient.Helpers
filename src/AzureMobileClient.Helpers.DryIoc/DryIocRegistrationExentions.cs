using DryIoc;
using System;
using System.Reflection;

namespace AzureMobileClient.Helpers
{
    public static class DryIocRegistrationExtensions
    {
        public static IContainer RegisterSyncTableProvider(this IContainer container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudSyncTable<>));

        public static IContainer RegisterSyncTableProvider(this IContainer container, Type type)
        {
            if(type == null)
            {
                throw new NullArgumentException(nameof(type));
            }

            if(!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("The specified type must a Generic type");
            }

            if(type.GetTypeInfo().GetGenericTypeDefinition() != typeof(ICloudSyncTable<>))
            {
                throw new ArgumentException("The specified type must be of type 'ICloudSyncTable<>'");
            }

            container.Register(typeof(ICloudSyncTable<>), type);
            return container;
        }

        public static IContainer RegisterSyncTableProvider(this IContainer container) =>
            container.RegisterSyncTableProvider(typeof(AzureCloudTable<>));

        public static IContainer RegisterTableProvider(this IContainer container, Type type)
        {
            if(type == null)
            {
                throw new NullArgumentException(nameof(type));
            }

            if(!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("The specified type must a Generic type");
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