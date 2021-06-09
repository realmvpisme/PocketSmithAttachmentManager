using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Transactions.Mappers
{
    public class TransactionMapperProfile : Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<TransactionModel, DB_Transaction>().ReverseMap();
        }
    }
}