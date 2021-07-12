using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExportServices;
using PocketSmith.DataExportServices.Accounts;
using PocketSmith.DataExportServices.Budget;
using PocketSmith.DataExportServices.Categories;
using PocketSmith.DataExportServices.Institutions;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmith.DataExportServices.Transactions;
using PocketSmithAttachmentManager.Menus;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.WebServices
{
    public class DataDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly TransactionService _transactionService;
        private readonly ContextFactory _contextFactory;
        private TransactionDataService _transactionDataService;
        private AccountDataService _accountDataService;
        private CategoryDataService _categoryDataService;
        private InstitutionDataService _institutionDataService;
        private readonly CategoryService _categoryService;
        private readonly BudgetService _budgetService;
        private ScenarioDataService _scenarioDataService;
        private BudgetEventDataService _budgetEventDataService;
        private readonly TransactionAccountService _transactionAccountService;
        private TransactionAccountDataService _transactionAccountDataService;
        private readonly AccountService _accountService;
        private AccountBalanceDataService _accountBalanceDataService;

        public DataDownloadService()
        {
            _httpClient = new HttpClient();
            _httpClient
                .DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            _httpClient
                .DefaultRequestHeaders
                .Add("Accept", "application/json");


            _transactionService = new TransactionService();
            _contextFactory = new ContextFactory();
            _categoryService = new CategoryService();
            _budgetService = new BudgetService();
            _accountService = new AccountService();
            _transactionAccountService = new TransactionAccountService();
        }

        public async Task DownloadAccounts(bool isInlineMethod = false)
        {

            var apiAccounts = await _accountService.GetAll();

            var apiTransactionAccounts = await _transactionAccountService.GetAll();

            var apiScenarios = apiAccounts.Select(x => x.PrimaryScenario).ToList();

            var apiInstitutions = apiTransactionAccounts.Select(x => x.Institution).ToList();



            var entityCount = apiAccounts.Count + apiTransactionAccounts.Count + apiScenarios.Count + apiInstitutions.Count;

            var progressBarOptions = new ProgressBarOptions()
            {
                CollapseWhenFinished = true,
                ForegroundColor = ConsoleColor.White
            };

            using var progressBar = new ProgressBar(entityCount, "Adding Accounts to Database", progressBarOptions);

            await processInstitutions(apiInstitutions, progressBar);
            await processAccounts(apiAccounts, progressBar);
            await processTransactionAccounts(apiTransactionAccounts, progressBar);
            await processScenarios(apiScenarios, progressBar);



            progressBar.Dispose();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All accounts downloaded successfully!");
            Console.ForegroundColor = ConsoleColor.White;

            if (!isInlineMethod)
            {
                Console.WriteLine("Returning to main menu...");

                Thread.Sleep(5000);

                await MainMenu.Show();
            }
        }

        public async Task DownloadBudgetEvents(bool isInlineMethod = false)
        {
            var apiBudgetEvents = await _budgetService.GetAll();

            var apiBudgetCategories = await resolveCategories(apiBudgetEvents
                .Select(x => x.Category)
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .OrderBy(x => x.Id)
                .ThenBy(x => x.ParentId)
                .ToList());

            var apiScenarios = apiBudgetEvents.Select(x => x.Scenario).ToList();

            var entityCount = apiBudgetEvents.Count + apiBudgetCategories.Count + apiScenarios.Count;

            var progressBarOptions = new ProgressBarOptions()
            {
                CollapseWhenFinished = true,
                ForegroundColor = ConsoleColor.White
            };

            using var progressBar = new ProgressBar(entityCount, "Adding Budget Events to Database", progressBarOptions);

            await processCategories(apiBudgetCategories, progressBar);
            await processScenarios(apiScenarios, progressBar);
            await processBudgetEvents(apiBudgetEvents, progressBar);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All budget events downloaded successfully!");
            Console.ForegroundColor = ConsoleColor.White;

            if (!isInlineMethod)
            {
                Console.WriteLine("Returning to main menu...");

                Thread.Sleep(5000);

                await MainMenu.Show();
            }
        }

        public async Task DownloadTransactions(bool isInlineMethod = false)
        {
            Console.Clear();

            var apiTransactions = await _transactionService.GetAll();

            var apiCategories = await resolveCategories(apiTransactions
                .Select(x => x.Category)
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .OrderBy(x => x.Id)
                .ThenBy(x => x.ParentId)
                .ToList());

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
            var progressBarOptions = new ProgressBarOptions()
            {
                CollapseWhenFinished = true,
                ForegroundColor = ConsoleColor.White
            };
            using var progressBar = new ProgressBar(entityCount, "Adding Transactions to Database", progressBarOptions);

            await processCategories(apiCategories, progressBar);
            await processInstitutions(apiInstitutions, progressBar);
            await processTransactionAccounts(apiAccounts, progressBar);
            await processTransactions(apiTransactions, progressBar);

            progressBar.Dispose();

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All transactions downloaded successfully!");
            Console.ForegroundColor = ConsoleColor.White;

            if (!isInlineMethod)
            {
                Console.WriteLine("Returning to main menu...");

                Thread.Sleep(5000);

                await MainMenu.Show();
            }
        }

        public async Task DownloadAllData()
        {
            await DownloadAccounts(true);
            await DownloadTransactions(true);
            await DownloadBudgetEvents(true);

            Console.WriteLine("Returning to main menu...");

            Thread.Sleep(5000);

            await MainMenu.Show();
        }

        public async Task<bool> LoadDatabase(string filePath)
        {
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
                Environment.Exit((int)ExitCodes.InvalidDatabase);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database file loaded successfully.");
            Console.ForegroundColor = ConsoleColor.White;

            //Create service if database file was found.
            _transactionDataService = new TransactionDataService(filePath);
            _accountDataService = new AccountDataService(filePath);
            _categoryDataService = new CategoryDataService(filePath);
            _institutionDataService = new InstitutionDataService(filePath);
            _scenarioDataService = new ScenarioDataService(filePath);
            _budgetEventDataService = new BudgetEventDataService(filePath);
            _accountBalanceDataService = new AccountBalanceDataService(filePath);
            _transactionAccountDataService = new TransactionAccountDataService(filePath);

            return true;
        }


        private async Task processAccounts(List<AccountModel> apiAccounts, ProgressBar progressBar)
        {
            var dbAccounts = await _accountDataService.GetAll();

            foreach (var account in apiAccounts)
            {
                if (!await _transactionAccountDataService.Exists(account.PrimaryTransactionAccount.Id))
                {
                    account.PrimaryTransactionAccount = null;
                }
                //Prevents circular dependency between accounts and scenarios.
                if (!await _scenarioDataService.Exists(Convert.ToInt64(account.PrimaryScenario.Id)))
                {
                    account.PrimaryScenario = null;
                }
                progressBar.Tick();
                var selectedDbAccount = dbAccounts.FirstOrDefault(x => x.Id == account.Id);

                if (selectedDbAccount == null)
                {
                    var createResult = await _accountDataService.Create(account);
                    dbAccounts.Add(createResult);

                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<AccountModel>();
                    comparer.IgnoreMember(x => x.Name == "Scenarios");
                    comparer.IgnoreMember(x => x.Name == "TransactionAccounts");
                    comparer.IgnoreMember(x => x.Name == "PrimaryScenario");
                    comparer.IgnoreMember(x => x.Name == "PrimaryTransactionAccount");

                    var accountsEqual = comparer.Compare(account, selectedDbAccount);

                    if (!accountsEqual)
                    {
                        await _accountDataService.Update(account, account.Id);
                    }
                }

                //Add Account Balance
                var accountBalance = new AccountBalanceModel()
                {
                    AccountId = account.Id,
                    Balance = account.CurrentBalance,
                    Date = (DateTime)account.CurrentBalanceDate
                };

                await _accountBalanceDataService.Create(accountBalance);
            }

        }
        private async Task processBudgetEvents(IEnumerable<BudgetEventModel> apiBudgetEvents, ProgressBar progressBar)
        {
            var dbBudgetEvents = await _budgetEventDataService.GetAll();

            foreach (var budgetEvent in apiBudgetEvents)
            {
                progressBar.Tick();
                var selectedDbBudgetEvent = dbBudgetEvents.FirstOrDefault(x => x.Id == budgetEvent.Id);

                if (selectedDbBudgetEvent == null)
                {
                    var createResult = await _budgetEventDataService.Create(budgetEvent);
                    dbBudgetEvents.Add(createResult);
                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<BudgetEventModel>();
                    comparer.IgnoreMember(x => x.Name == "Scenario");
                    comparer.IgnoreMember(x => x.Name == "Category");

                    var budgetEventsEqual = comparer.Compare(budgetEvent, selectedDbBudgetEvent);

                    if (!budgetEventsEqual)
                    {
                        await _budgetEventDataService.Update(budgetEvent, budgetEvent.Id);
                    }
                }
            }

        }

        private async Task processScenarios(List<ScenarioModel> apiScenarios, ProgressBar progressBar)
        {
            var dbScenarios = await _scenarioDataService.GetAll();


            foreach (var scenario in apiScenarios)
            {
                //Prevents circular dependency between accounts and scenarios.
                if (!await _accountDataService.Exists(Convert.ToInt64(scenario.AccountId)))
                {
                    scenario.AccountId = null;
                }
                
                progressBar.Tick();
                var selectedDbScenario = dbScenarios.FirstOrDefault(x => x.Id == scenario.Id);

                if (selectedDbScenario == null)
                {
                    var createResult = await _scenarioDataService.Create(scenario);
                    dbScenarios.Add(createResult);
                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<ScenarioModel>();
                    comparer.IgnoreMember(x => x.Name == "BudgetEvents");
                    var scenariosEqual = comparer.Compare(scenario, selectedDbScenario);

                    if (!scenariosEqual)
                    {
                        await _scenarioDataService.Update(scenario, scenario.Id);
                    }
                }
            }
        }

        private async Task processTransactionAccounts(IEnumerable<TransactionAccountModel> apiAccounts, ProgressBar progressBar)
        {
            var dbAccounts = await _transactionAccountDataService.GetAll();

            foreach (var account in apiAccounts)
            {
                if (!await _accountDataService.Exists(Convert.ToInt64(account.AccountId)))
                {
                    account.AccountId = null;
                }
                progressBar.Tick();
                var selectedDbAccount = dbAccounts.FirstOrDefault(x => x.Id == account.Id);

                if (selectedDbAccount == null)
                {
                    var createResult = await _transactionAccountDataService.Create(account);
                    dbAccounts.Add(createResult);
                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<TransactionAccountModel>();
                    comparer.IgnoreMember(x => x.Name == "Institution");
                    var accountsEqual = comparer.Compare(account, selectedDbAccount);

                    if (!accountsEqual)
                    {
                        await _transactionAccountDataService.Update(account, account.Id);
                    }
                }
            }
        }

        private async Task processCategories(IEnumerable<CategoryModel> apiCategories, ProgressBar progressBar)
        {
            var dbCategories = await _categoryDataService.GetAll();

            foreach (var category in apiCategories.Where(x => x != null))
            {
                progressBar.Tick();
                var selectedDbCategory = dbCategories.FirstOrDefault(x => x.Id == category.Id);
                if (selectedDbCategory == null)
                {
                    var createResult = await _categoryDataService.Create(category);
                    dbCategories.Add(createResult);
                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<CategoryModel>();
                    comparer.IgnoreMember(x => x.Name == "Children");
                    comparer.IgnoreMember(x => x.Name == "BudgetEvents");
                    var categoriesEqual = comparer.Compare(category, selectedDbCategory);

                    if (!categoriesEqual)
                    {
                        await _categoryDataService.Update(category, category.Id);
                    }
                }
            }
        }

        private async Task processInstitutions(IEnumerable<InstitutionModel> apiInstitutions, ProgressBar progressBar)
        {
            var dbInstitutions = await _institutionDataService.GetAll();

            foreach (var institution in apiInstitutions)
            {
                progressBar.Tick();

                var selectedInstitution = dbInstitutions.FirstOrDefault(x => x.Id == institution.Id);
                if (selectedInstitution == null)
                {
                    var createResult = await _institutionDataService.Create(institution);
                    dbInstitutions.Add(createResult);
                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<InstitutionModel>();
                    var institutionsEqual = comparer.Compare(institution, selectedInstitution);
                    if (!institutionsEqual)
                    {
                        await _institutionDataService.Update(institution, institution.Id);
                    }
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
                progressBar.Tick();

                var selectedDbTransaction = dbTransactions.FirstOrDefault(x => x.Id == apiTransaction.Id);
                if (selectedDbTransaction == null)
                {
                    var createResult = await _transactionDataService.Create(apiTransaction);
                    dbTransactions.Add(createResult);
                }
                else
                {
                    var comparer = new ObjectsComparer.Comparer<TransactionModel>();
                    comparer.IgnoreMember(x => x.Name == "Category");
                    comparer.IgnoreMember(x => x.Name == "TransactionAccount");
                    var transactionsEqual = comparer.Compare(apiTransaction, selectedDbTransaction);
                    if (!transactionsEqual)
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

        private async Task<List<CategoryModel>> resolveCategories(List<CategoryModel> apiCategories)
        {
            //The "Category" table is self referencing so there is a specific order in which categories must be inserted into the database.
            //This method re-orders the categories to ensure that a sql exception is not thrown.
            Console.WriteLine("Resolving categories...");
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