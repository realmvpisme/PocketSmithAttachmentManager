using System;
using System.Linq;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.Menus
{
    public static class DownloadDataMenu
    {
        public static string MenuText = @"
        *****Download Data*****

    1. Download All Data
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
        }
    }
}