using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.Services;

namespace PocketSmithAttachmentManager.Menus
{
    public static class TransactionMenu
    {
        private static readonly TransactionService _transactionService;

        static TransactionMenu()
        {
            _transactionService = new TransactionService();
        }

        public static string MenuText { get; } = @"
        *****Manage Transactions*****

    1. Manage Transaction Attachments
    2. Download Transactions
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
            } while (!Enumerable.Range(1, 3).Contains(selectedOption));

            switch (selectedOption)
            {
                case 1:
                {
                    break;
                }

                case 2:
                {

                    break;
                }

                case 3:
                {
                    await MainMenu.Show();
                    break;
                }
            }
        }
    }
}