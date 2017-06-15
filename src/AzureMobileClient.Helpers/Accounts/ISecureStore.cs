using System;

namespace AzureMobileClient.Helpers.Accounts
{
    public interface ISecureStore
	{
		string this[string key] { get; set; }
	}
}