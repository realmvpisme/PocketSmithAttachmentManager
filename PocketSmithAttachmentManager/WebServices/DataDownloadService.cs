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

            var existingTransactions = await _transactionDataService.GetAll();

            var transactions = await _transactionService.GetAllTransactions();



            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            using var progressBar = new ProgressBar(transactions.Count, "Adding Transactions to Database", ConsoleColor.White);

            

            foreach (var transaction in transactions)
            {
                progressBar.Tick();

                //Add transaction account.
                if (transaction.TransactionAccount != null)
                {
                    await processTransactionAccounts(transaction.TransactionAccount);
                }

                //Add category.
                if (transaction.Category != null)
                {
                    await processCategories(transaction.Category);
                }

                await processTransactions(transactions, existingTransactions);
                
            }

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

        private async Task processTransactionAccounts(IEnumerable<AccountModel> apiAccounts)
        {
            var dbAccounts = await _accountDataService.GetAll();

            foreach (var account in apiAccounts)
            {
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

        private async Task processCategories(IEnumerable<CategoryModel> apiCategories)
        {
            var dbCategories = await _categoryDataService.GetAll();

            foreach (var category in apiCategories)
            {
                if (!dbCategories.Any(x => x.Id == category.Id))
                {
                   await _categoryDataService.Create(category);
                }
                else
                {
                    var dbCategory = dbCategories.FirstOrDefault(x => x.Id == category.Id);
                    if (dbCategory != category)
                    {
                        await _categoryDataService.Update(category);
                    }
                }
            }


            //Process child categories.

            foreach (var childCategory in category.Children)
            {
                if (await categoryDataService.Exists(childCategory.Id))
                {
                    existingCategories.Add(childCategory);
                }
                else
                {
                    await categoryDataService.Create(childCategory);
                }
            }
        }

        private async Task processInstitutions(IEnumerable<InstitutionModel> apiInstitutions)
        {
           

            var existingInstitutions = await _institutionDataService.GetAll();

            var existingInstitution = existingInstitutions.FirstOrDefault(x => x.Id == institution.Id);

            if (existingInstitution == null)
            {
                await _institutionDataService.Create(institution);
                return;
            }

            if (existingInstitution != institution)
            {
                await _institutionDataService.Update(institution, institution.Id);
            }

        }

        private async Task processTransactions(IEnumerable<TransactionModel> apiTransactions)
        {
            var dbInstitutions = await _institutionDataService.GetAll();
            var dbCategories = await _categoryDataService.GetAll();
            var dbAccounts = await _accountDataService.GetAll();

            var dbTransactions = await _transactionDataService.GetAll();

            var apiCategories = new List<CategoryModel>();
            var apiAccounts = new List<AccountModel>();
            var apiInstitutions = new List<InstitutionModel>();

            foreach (var transaction in apiTransactions)
            {
                //Process categories.
                if (transaction.Category != null && apiCategories.All(x => x.Id != transaction.Category.Id))
                {
                    apiCategories.Add(transaction.Category);
                }

                //Process accounts.
                if (transaction.TransactionAccount != null &&
                    apiAccounts.All(x => x.Id != transaction.TransactionAccount.Id))
                {
                    apiAccounts.Add(transaction.TransactionAccount);
                }
            }

            //Process institutions.
            foreach (var account in apiAccounts)
            {
                if (account != null && apiInstitutions.All(x => x.Id != account.Institution.Id))
                {
                    apiInstitutions.Add(account.Institution);
                }
            }
            await processInstitutions(apiInstitutions);
            await processTransactionAccounts(apiAccounts);
            await processCategories(apiCategories);


            Console.WriteLine("Creating new database transactions...");

            //Check for existing transaction in database. Create if one does not exist.
            foreach (var apiTransaction in apiTransactions)
            {


                if (!databaseTransactions.Any(x => x.Id == apiTransaction.Id))
                {
                   await _transactionDataService.Create(apiTransaction);
                }
                
            }

            Console.WriteLine("Cleaning up database transactions...");

            //Update existing transactions if needed.
            foreach (var transaction in databaseTransactions)
            {
                var apiTransaction = apiTransactions.FirstOrDefault(x => x.Id == transaction.Id);

                if (apiTransaction != transaction)
                {
                    await _transactionDataService.Update(apiTransaction, transaction.Id);
                }

            }

            //Delete database transactions that no longer exist in the API.
            foreach (var dbTransaction in databaseTransactions)
            {
                if (!apiTransactions.Any(x => x.Id.Equals(dbTransaction.Id)))
                {
                   await _transactionDataService.Delete(dbTransaction.Id);
                }
            }

        }
    }
}