using System;
using System.Collections.Generic;

namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    /// <summary>
    /// The AAD Options
    /// </summary>
    public interface IAADOptions
    {
        /// <summary>
        /// The Redirect Uri
        /// </summary>
        string RedirectUri { get; }

        /// <summary>
        /// The Login Authority
        /// </summary>
        string Authority { get; }

        /// <summary>
        /// The Scopes
        /// </summary>
        IEnumerable<string> Scopes { get; }

        /// <summary>
        /// The Default Policy
        /// </summary>
        string Policy { get; }
    }
}
