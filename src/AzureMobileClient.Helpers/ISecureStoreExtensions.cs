using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Akavache;
using System.Reactive;
using System.Reactive.Linq;

namespace AzureMobileClient.Helpers
{
    internal static class ISecureBlobCacheExtensions
    {
        public static IObservable<bool> ContainsKey(this ISecureBlobCache secureCache, string key) => 
            Observable.FromAsync(async () =>
            {
                var keys = await secureCache.GetAllKeys();
                return keys.Any(k => k == key);
            });
    }
}
