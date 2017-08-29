using System;
using System.Collections.Generic;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    public interface IAADOptions
    {
        string RedirectUri { get; }

        string Authority { get; }

        IEnumerable<string> Scopes { get; }

        string Policy { get; }
    }
}
