using System;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.Menus;

namespace PocketSmithAttachmentManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "PocketSmith Attachment Manager v2.0";

            await MainMenu.Show();
        }
    }
}
