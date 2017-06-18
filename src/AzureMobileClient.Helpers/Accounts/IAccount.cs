
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMobileClient.Helpers.Accounts
{

    public interface IAccount : IDictionary<string, string>
    {
        string Id { get; set; }
        string Name { get; set; }
        bool IsValid { get; }
        Task<bool> CheckValidity();
    }
}