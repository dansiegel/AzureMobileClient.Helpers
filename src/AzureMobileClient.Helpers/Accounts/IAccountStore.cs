using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AzureMobileClient.Helpers.Accounts
{
    public interface IAccountStore
    {
        IAccount FindAccount(string providerType, string id);
        IEnumerable<IAccount> FindAccounts(string providerType);
        IAccount FindAnyAccount(string providerType);
        void SaveAccount(string providerType, IAccount account);
        void DeleteAccount(string providerType, string id);

        ISecureStore SecureStore { get; set; }
    }
}