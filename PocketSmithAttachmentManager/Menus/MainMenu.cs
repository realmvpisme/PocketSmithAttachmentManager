using System;
using System.Linq;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.Menus
{
    public static class MainMenu
    {
        public static string MenuText { get; } = @"
        *****Main Menu*****

    1. Add Local Attachment
    2. Manage Attachment Inbox
    3. Manage Transaction Attachments
    4. Exit

";

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
                        await LocalAttachmentMenu.Show();
                        break;
                    }

                case 2:
                    {
                        await AttachmentInboxMenu.Show();
                        break;
                    }

                case 3:
                    {
                        await TransactionMenu.Show();
                        break;
                    }

                case 4:
                    {
                        Environment.Exit(0);
                        break;
                    }
            }
        }
    }
}