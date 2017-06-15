using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AzureMobileClient.Helpers.Accounts
{
    public class AccountStore : IAccountStore
    {
        public AccountStore(ISecureStore secureStore = null)
        {
            SecureStore = secureStore;
        }

        public ISecureStore SecureStore { get; set; }

        public IAccount FindAccount(string providerTypeId, string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var accounts = FindAccounts(providerTypeId);

            return accounts.FirstOrDefault(a => a.Id == id);
        }

        // public IAccount FindAnyAccount(ProviderType providerType)
        // {
        //     return FindAnyAccount(SocialAuthManager.GetProviderTypeId(providerType));
        // }

        public IAccount FindAnyAccount(string providerTypeId)
        {
            var accounts = FindAccounts(providerTypeId);

            return accounts?.FirstOrDefault();
        }

        // public IEnumerable<IAccount> FindAccounts(ProviderType providerType)
        // {
        //     return FindAccounts(SocialAuthManager.GetProviderTypeId(providerType));
        // }

        public IEnumerable<IAccount> FindAccounts(string providerTypeId)
        {
            var serialized = SecureStore?[providerTypeId];

            var items = JsonSerializer.Deserialize<List<IAccount>>(serialized, false, true);

            return items;
        }

        public void SaveAccount(string providerTypeId, IAccount account)
        {
            var accounts = new List<IAccount>();

            var serialized = SecureStore?[providerTypeId];
            if (!string.IsNullOrEmpty(serialized))
                accounts = JsonSerializer.Deserialize<List<IAccount>>(serialized);

            if (account != null && !string.IsNullOrEmpty(account.Id))
                accounts.RemoveAll(a => a.Any(kvp => kvp.Key == "id" && kvp.Value == account.Id));

            accounts.Add(account);

            serialized = JsonSerializer.Serialize(accounts, true, true);

            SecureStore[providerTypeId] = serialized;
        }

        public void DeleteAccount(string providerTypeId, string id)
        {
            var accounts = JsonSerializer.Deserialize<List<IAccount>>(SecureStore?[providerTypeId]);

            accounts.RemoveAll(a => a.Id == id);

            var serialized = JsonSerializer.Serialize(accounts, true, true);

            SecureStore[providerTypeId] = serialized;
        }
    }
}