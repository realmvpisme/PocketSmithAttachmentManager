using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;

namespace PocketSmithAttachmentManager.WebServices
{
    public class CategoryService : WebServiceBase<CategoryModel, long>
    {
        public CategoryService() : base(PocketSmithUri.TRANSACTIONS, EntityType.TRANSACTIONS)
        {

        }
        public override async Task<List<CategoryModel>> GetAll()
        {
            var uri = PocketSmithUri.ALL_CATEGORIES;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);
            //The category API endpoint is unique in that all the categories are nested. First query returns all top level categories
            //with child categories nested. 

            var httpResponse = await RestClient.Get(uri);
            var categories = JsonSerializer.Deserialize<List<CategoryModel>>(httpResponse, SerializerOptions);

            var categoryList = getAllCategories(categories);

            
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

        public List<CategoryModel> getAllCategories(List<CategoryModel> categories)
        {
            var categoryList = new List<CategoryModel>();
            foreach (var category in categories)
            {
                if (category.Children.Any())
                {
                    var childCategories = getAllCategories(category.Children);
                    categoryList.AddRange(childCategories);
                    categoryList.Add(category);
                }
                else
                {
                    categoryList.Add(category);
                }

            }

            return categoryList;
        }

    }
}