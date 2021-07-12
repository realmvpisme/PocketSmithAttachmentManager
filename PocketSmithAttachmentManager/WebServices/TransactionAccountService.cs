using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;
using System.Text.Json;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.WebServices
{
    public class TransactionAccountService : WebServiceBase<TransactionAccountModel, long>
    {
        public TransactionAccountService() : base(PocketSmithUri.TRANSACTION_ACCOUNTS_ALL, EntityType.TRANSACTION_ACCOUNTS)
        {

        }

        public override async Task<TransactionAccountModel> GetById(long id)
        {
            var uri = PocketSmithUri.TRANSACTION_ACCOUNT_BY_ID;
            uri = uri.Replace("{id}", id.ToString());

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var transactionAccount = JsonSerializer.Deserialize<TransactionAccountModel>(httpResponse, SerializerOptions);

            return transactionAccount;
        }
    }
}