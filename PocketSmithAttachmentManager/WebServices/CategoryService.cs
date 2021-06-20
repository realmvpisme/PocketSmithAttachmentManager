using System.Collections.Generic;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;

namespace PocketSmithAttachmentManager.WebServices
{
    public class CategoryService : WebServiceBase<CategoryModel, long>
    {
        public CategoryService() : base(PocketSmithUri.TRANSACTIONS)
        {

        }
        public override async Task<List<CategoryModel>> GetAll()
        {
            var uri = PocketSmithUri.ALL_CATEGORIES;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            var categoryList = new List<CategoryModel>();

            var httpResponse = await RestClient.Get(uri);
            var categories = JsonSerializer.Deserialize<List<CategoryModel>>(httpResponse, SerializerOptions);

            categories.ForEach(c => categoryList.Add(c));

            do
            {
                if (!string.IsNullOrEmpty(RestClient.CurrentPageUri))
                {
                    RestClient.PreviousPageUri = RestClient.CurrentPageUri;
                }

                if (!string.IsNullOrEmpty(RestClient.NextPageUri))
                {
                    RestClient.CurrentPageUri = RestClient.NextPageUri;
                }

                httpResponse = await RestClient.Get(RestClient.CurrentPageUri);
                categories = JsonSerializer.Deserialize<List<CategoryModel>>(httpResponse, SerializerOptions);
                categories.ForEach(t => categoryList.Add(t));

                //ToDo: Remove transaction count criteria.
            } while (RestClient.CurrentPageUri != RestClient.LastPageUri);

            return categoryList;
        }

        public override async Task<CategoryModel> GetById(long id)
        {
            var uri = PocketSmithUri.CATEGORY_BY_ID;
            uri = uri.Replace("{id}", id.ToString());

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var category = JsonSerializer.Deserialize<CategoryModel>(httpResponse, SerializerOptions);
            return category;
        }

    }
}