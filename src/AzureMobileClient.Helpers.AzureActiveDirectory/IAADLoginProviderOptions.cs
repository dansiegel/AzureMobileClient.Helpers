namespace AzureMobileClient.Helpers.AzureActiveDirectory
{
    /// <summary>
    /// The options for the AAD Login Provider
    /// </summary>
    public interface IAADLoginProviderOptions
    {
        /// <summary>
        /// The Directory Name
        /// </summary>
        string DirectoryName { get; }
    }
}