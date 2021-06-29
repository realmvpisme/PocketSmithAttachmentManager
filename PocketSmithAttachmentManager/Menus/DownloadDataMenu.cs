using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.WebServices;

namespace PocketSmithAttachmentManager.Menus
{
    public static class DownloadDataMenu
    {
        private static readonly DataDownloadService _dataDownloadService;

        static DownloadDataMenu()
        {
            _dataDownloadService = new DataDownloadService(typeof(DownloadDataMenu));
        }
        public static string MenuText = @"
        *****Download Data*****

    1. Download All Data
    2. Download Transactions
    3. Download Budget Events
    3. Return To Main Menu";

        public static async Task Show()
        {
            Console.Clear();
            Console.WriteLine(MenuText);

            int selectedOption = 0;
            do
            {
                Console.WriteLine("Please select an option and press ENTER:");
                try
                {
                    selectedOption = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    selectedOption = 0;
                }
            } while (!Enumerable.Range(1, 4).Contains(selectedOption));

            switch (selectedOption)
            {
                case 1:
                {
                    await downloadAllData();
                    break;
                }

                case 2:
                {
                    await downloadTransactions();
                    break;
                }

                case 3:
                {
                    await downloadBudgetEvents();
                    break;
                }

                case 4:
                {
                    await MainMenu.Show();
                    break;
                }
            }
        }

        private static async Task downloadAllData()
        {
            await prepareDatabase();

            await _dataDownloadService.DownloadAllData();

        }

        private static async Task downloadTransactions()
        {
            await prepareDatabase();
            await _dataDownloadService.DownloadTransactions();

        }

        private static async Task downloadBudgetEvents()
        {
            await prepareDatabase();
            await _dataDownloadService.DownloadBudgetEvents();
        }

        private static async Task prepareDatabase()
        {
            string databaseFilePath = null;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter the file path for an existing or new SQLite database or [C] to cancel and press ENTER:");
                var filePath = Console.ReadLine().TrimStart((@"""\\").ToCharArray()).TrimEnd((@"\\""").ToCharArray());
                if (Directory.Exists(Path.GetDirectoryName(filePath)) || filePath.ToLower() == "c")
                {
                    databaseFilePath = filePath;
                }
            } while (string.IsNullOrEmpty(databaseFilePath));

            if (databaseFilePath.ToLower() == "c")
            {
                await Show();
            }

            await _dataDownloadService.LoadDatabase(databaseFilePath);
        }

    }
}