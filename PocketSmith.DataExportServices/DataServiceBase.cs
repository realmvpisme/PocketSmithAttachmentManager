using System;
using System.Threading.Tasks;

namespace PocketSmith.DataExportServices
{
    public abstract class DataServiceBase<TJsonModel, TDatabaseModel>
    {
        public virtual async Task Create(TJsonModel createItem)
        {
            throw new NotImplementedException();
        }

        public virtual async Task Update(TJsonModel updateItem)
        {
            throw new NotImplementedException();
        }

        public virtual async Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TDatabaseModel> GetById(long id)
        {
            throw new NotImplementedException();
        }
    }
}