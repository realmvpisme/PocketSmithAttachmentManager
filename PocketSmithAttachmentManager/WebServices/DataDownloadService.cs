using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExportServices.Accounts;
using PocketSmith.DataExportServices.Categories;
using PocketSmith.DataExportServices.Institutions;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmith.DataExportServices.Transactions;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ObjectsComparer;

namespace PocketSmithAttachmentManager.WebServices
{
    public class DataDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly Type _parentMenuType;
        private readonly TransactionService _transactionService;
        private readonly ContextFactory _contextFactory;
        private string _databaseFilePath;
        private TransactionDataService _transactionDataService;
        private AccountDataService _accountDataService;
        private CategoryDataService _categoryDataService;
        private InstitutionDataService _institutionDataService;
        private CategoryService _categoryService;


        public DataDownloadService(Type parentMenuType)
        {
            _httpClient = new HttpClient();
            _httpClient
                .DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            _httpClient
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            _parentMenuType = parentMenuType;

            _transactionService = new TransactionService();
            _contextFactory = new ContextFactory();
            _categoryService = new CategoryService();
        }

        public async Task DownloadAllData()
        {
            var context = _contextFactory.Create(_databaseFilePath);

            Console.Clear();

            var apiTransactions = await _transactionService.GetAll();

            var apiCategories = await resolveCategories(apiTransactions);
            
            var apiAccounts = apiTransactions
                .Select(x => x.TransactionAccount)
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .ToList();

            var apiInstitutions = apiTransactions
                .Select(x => x.TransactionAccount)
                .Where(x => x != null)
                .Select(y => y.Institution)
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .ToList();


            var entityCount = apiTransactions.Count() + apiCategories.Count() + apiInstitutions.Count() +
                              apiAccounts.Count();


            using var progressBar = new ProgressBar(entityCount, "Adding Transactions to Database", ConsoleColor.White);

            await processCategories(apiCategories, progressBar);
            await processInstitutions(apiInstitutions, progressBar);
            await processTransactionAccounts(apiAccounts, progressBar);
            await processTransactions(apiTransactions, progressBar);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All transactions downloaded successfully!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async Task<bool> LoadDatabase(string filePath)
        {
            _databaseFilePath = filePath;

            _transactionDataService = new TransactionDataService(filePath);
            _accountDataService = new AccountDataService(filePath);
            _categoryDataService = new CategoryDataService(filePath);
            _institutionDataService = new InstitutionDataService(filePath);

            await using var context = _contextFactory.Create(filePath);

            bool fileExists = File.Exists(filePath);

            Console.Clear();
            Console.WriteLine(fileExists
                ? "Loading database file..."
                : "Database file does not exist. Creating new database file...");

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Database file failed to load.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database file loaded successfully.");
            Console.ForegroundColor = ConsoleColor.White;

            return true;
        }

        private async Task processTransactionAccounts(IEnumerable<AccountModel> apiAccounts, ProgressBar progressBar)
        {
            var dbAccounts = await _accountDataService.GetAll();


            foreach (var account in apiAccounts)
            {
                progressBar.Tick();

                if (!dbAccounts.Any(x => x.Id == account.Id))
                {
                    await _accountDataService.Create(account);
                }
                else
                {
                    var dbAccount = dbAccounts.FirstOrDefault(x => x.Id == account.Id);
                    if (dbAccount != account)
                    {
                        await _accountDataService.Update(account, account.Id);
                    }
                }
            }

            foreach (var dbAccount in dbAccounts)
            {
                if (!apiAccounts.Any(x => x.Id == dbAccount.Id))
                {
                    await _accountDataService.Delete(dbAccount.Id);
                }
            }
            
        }

        private async Task processCategories(IEnumerable<CategoryModel> apiCategories, ProgressBar progressBar)
        {
            var dbCategories = await _categoryDataService.GetAll();

            foreach (var category in apiCategories.Where(x => x != null))
            {
                progressBar.Tick();

                if (!dbCategories.Any(x => x.Id == category.Id))
                {
                    await _categoryDataService.Create(category);
                }
                else
                {
                    var dbCategory = dbCategories.FirstOrDefault(x => x.Id == category.Id);

                    var comparer = new ObjectsComparer.Comparer<CategoryModel>();
                    IEnumerable<Difference> differences;
                    var categoriesEqual = comparer.Compare(category, dbCategory, out differences);

                    if (!categoriesEqual)
                    {
                        await _categoryDataService.Update(category, category.Id);
                    }
                }

            }

            //Delete categories that don't exist in the API.
            foreach (var dbCategory in dbCategories)
            {
                if (!apiCategories.Any(x => x.Id == dbCategory.Id) && !apiCategories.SelectMany(x => x.Children).Any(y => y.Id == dbCategory.Id))
                {
                    await _categoryDataService.Delete(dbCategory.Id);
                }
            }
        }

        private async Task processInstitutions(IEnumerable<InstitutionModel> apiInstitutions, ProgressBar progressBar)
        {
            var dbInstitutions = await _institutionDataService.GetAll();

            foreach (var institution in apiInstitutions)
            {
                progressBar.Tick();

                if (!apiInstitutions.Any(x => x.Id == institution.Id))
                {
                    await _institutionDataService.Create(institution);
                }
                else
                {
                    var dbInstitution = await _institutionDataService.GetById(institution.Id);
                    if (dbInstitution != institution)
                    {
                        await _institutionDataService.Update(institution, institution.Id);
                    }
                }
            }

            foreach (var dbInstitution in dbInstitutions)
            {
                if (!apiInstitutions.Any(x => x.Id == dbInstitution.Id))
                {
                    await _institutionDataService.Delete(dbInstitution.Id);
                }
            }
        }

        private async Task processTransactions(IEnumerable<TransactionModel> apiTransactions, ProgressBar progressBar)
        {

            var dbTransactions = await _transactionDataService.GetAll();

            Console.WriteLine("Creating new database transactions...");

            //Check for existing transaction in database. Create if one does not exist.
            foreach (var apiTransaction in apiTransactions)
            {
                if (!dbTransactions.Any(x => x.Id == apiTransaction.Id))
                {
                   await _transactionDataService.Create(apiTransaction);
                }
                else
                {
                    var dbTransaction = dbTransactions.FirstOrDefault(x => x.Id == apiTransaction.Id);
                    if (dbTransaction != apiTransaction)
                    {
                        await _transactionDataService.Update(apiTransaction, apiTransaction.Id);
                    }
                }
            }

            Console.WriteLine("Cleaning up database transactions...");

            //Delete database transactions that no longer exist in the API.
            foreach (var dbTransaction in dbTransactions)
            {
                if (!apiTransactions.Any(x => x.Id.Equals(dbTransaction.Id)))
                {
                   await _transactionDataService.Delete(dbTransaction.Id);
                }
            }

        }

        private async Task<IEnumerable<CategoryModel>> resolveCategories(IEnumerable<TransactionModel> transactions)
        {
            var apiCategories = transactions
                .Select(x => x.Category)
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .OrderBy(x => x.Id)
                .ThenBy(x => x.ParentId)
                .ToList();

            do
            {
                List<CategoryModel> missingCategories = new List<CategoryModel>();

                foreach (var category in apiCategories)
                {
                    if (category.ParentId != null && apiCategories.All(x => x.Id != category.ParentId))
                    {
                        var apiCategory = await _categoryService.GetById(Convert.ToInt64(category.ParentId));
                        missingCategories.Add(apiCategory);
                    }
                }
                apiCategories.AddRange(missingCategories);
            } while (apiCategories.Any(x => x.ParentId != null && apiCategories.All(y => y.Id != x.ParentId)));

            apiCategories = apiCategories.Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.ParentId)
                .ToList();

            var returnCategories = apiCategories
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.Id)
                .ToList();

            apiCategories.RemoveAll(x => returnCategories.Any(y => y.Id == x.Id));

            do
            {
                returnCategories.AddRange(apiCategories.Where(x => returnCategories.Any(y => y.Id == x.ParentId)));
                apiCategories.RemoveAll(x => returnCategories.Any(y => y.Id == x.Id));
            } while (apiCategories.Any());

            return returnCategories;
        }

    }
}