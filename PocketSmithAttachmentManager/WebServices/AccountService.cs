using System.Text.Json;
using System.Threading.Tasks;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;

namespace PocketSmithAttachmentManager.WebServices
{
    public class AccountService : WebServiceBase<AccountModel, long>
    {
        public AccountService() : base(PocketSmithUri.ACCOUNTS_ALL, EntityType.ACCOUNTS)
        {

        }

        public override async Task<AccountModel> GetById(long id)
        {
            var uri = PocketSmithUri.ACCOUNT_BY_ID;
            uri = uri.Replace("{id}", id.ToString());

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var transactionAccount = JsonSerializer.Deserialize<AccountModel>(httpResponse, SerializerOptions);

            return transactionAccount;
        }
    }
}