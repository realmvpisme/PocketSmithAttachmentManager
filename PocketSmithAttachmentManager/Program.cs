using System;
using System.Reflection;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.Menus;

namespace PocketSmithAttachmentManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = $"PocketSmith Attachment Manager v{Assembly.GetExecutingAssembly().GetName().Version}";

            await MainMenu.Show();
        }
    }
}
