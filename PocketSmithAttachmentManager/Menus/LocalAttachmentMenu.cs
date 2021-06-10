using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.WebServices;

namespace PocketSmithAttachmentManager.Menus
{
    public static class LocalAttachmentMenu
    {
        private static readonly AttachmentService _attachmentService;

        static LocalAttachmentMenu()
        {
            _attachmentService = new AttachmentService(typeof(LocalAttachmentMenu));
        }

        public static string MenuText { get; } =
            @"
        *****Local Attachments*****

    1. Upload Attachment
    2. Return To Main Menu
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
            } while (!Enumerable.Range(1, 2).Contains(selectedOption));

            switch (selectedOption)
            {
                case 1:
                    {
                        await uploadAttachment();
                        break;
                    }

                case 2:
                    {
                        await MainMenu.Show();
                        break;
                    }

            }
        }

        private static async Task uploadAttachment()
        {
            string attachmentFilePath = null;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter the file path of the attachment you wish to upload or [C] to cancel and press ENTER:");
                var filePath = Console.ReadLine().TrimStart((@"""\\").ToCharArray()).TrimEnd((@"\\""").ToCharArray());
                if (File.Exists(filePath) || filePath.ToLower() == "c")
                {
                    attachmentFilePath = filePath;
                }
            } while (string.IsNullOrEmpty(attachmentFilePath));

            if (attachmentFilePath.ToLower() == "c")
            {
                await LocalAttachmentMenu.Show();
            }

            var transactionMenuText = @"
Select Transaction Search Criteria:

    1. Amount
    2. Payee
    3. Date Range
    4. Return to Previous Menu
";
            Console.Clear();
            Console.WriteLine(transactionMenuText);

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
                        var selectedTransaction = await TransactionMenu.GetTransactionByAmount();
                        await _attachmentService.UploadAttachment(attachmentFilePath, selectedTransaction.Id);
                        break;
                    }

                case 2:
                {
                    var selectedTransaction = await TransactionMenu.GetTransactionByPayee();
                    await _attachmentService.UploadAttachment(attachmentFilePath, selectedTransaction.Id);
                    break;
                }

                case 3:
                {
                    var selectedTransaction = await TransactionMenu.GetTransactionByDate();
                    await _attachmentService.UploadAttachment(attachmentFilePath, selectedTransaction.Id);
                    break;
                }

                case 4:
                {
                    await LocalAttachmentMenu.Show();
                    break;
                }
            }
        }

        



       
    }
}