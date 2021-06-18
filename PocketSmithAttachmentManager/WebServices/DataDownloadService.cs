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
        }

        public async Task DownloadAllData()
        {
            var context = _contextFactory.Create(_databaseFilePath);

            Console.Clear();

            var transactions = await _transactionService.GetAllTransactions();


            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            var entityCount = transactions.Count() + transactions.Select(x => x.TransactionAccount).Count() +
                              transactions.Select(x => x.TransactionAccount.Institution).Count() + transactions.Select(x => x.Category).Count();

            using var progressBar = new ProgressBar(entityCount, "Adding Transactions to Database", ConsoleColor.White);


            await processTransactions(transactions, progressBar);

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
                    if (dbCategory != category)
                    {
                        await _categoryDataService.Update(category, category.Id);
                    }
                }


                //Process child categories.
                if (category?.Children != null)
                {
                    foreach (var childCategory in category.Children)
                    {
                        progressBar.Tick();

                        if (!dbCategories.Any(x => x.Id == childCategory.Id))
                        {
                            await _categoryDataService.Create(childCategory);
                        }
                        else
                        {
                            var dbCategory = dbCategories.FirstOrDefault(x => x.Id == childCategory.Id);
                            if (dbCategory != category)
                            {
                                await _categoryDataService.Update(childCategory, childCategory.Id);
                            }
                        }
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

            foreach (var transaction in apiTransactions)
            {
                progressBar.Tick();

                await processCategories(new List<CategoryModel>() {transaction.Category}, progressBar);
                await processInstitutions(new List<InstitutionModel>() {transaction.TransactionAccount.Institution}, progressBar);
                await processTransactionAccounts(new List<AccountModel>() {transaction.TransactionAccount}, progressBar);
            }

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
    }
}