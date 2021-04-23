using PocketSmithAttachmentManager.Models;
using PocketSmithAttachmentManager.Services;
using PocketSmithAttachmentManager.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.Menus
{
    public static class AttachmentInboxMenu
    {
        private static readonly AttachmentService _attachmentService;

        static AttachmentInboxMenu()
        {
            _attachmentService = new AttachmentService(typeof(AttachmentInboxMenu));
        }

        public static string MenuText { get; } = @"
        *****Attachment Inbox*****

    1. View Un-filed Attachments
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
                        await showUnfiledAttachments();
                        break;
                    }

                case 2:
                    {
                        await MainMenu.Show();
                        break;
                    }
            }
        }

        private static async Task showUnfiledAttachments()
        {
            var attachments = await _attachmentService.GetUnAssignedAttachments();

            var selectedAttachment = await selectAttachment(attachments);

            Console.Clear();
            Console.WriteLine($"Selected attachment {selectedAttachment.FileName}. \n");

            var attachmentMenuText = @"
Select Attachment Action:

    1. View Attachment
    2. Assign To Transaction
    3. Delete
    4. Return To Attachment Inbox Menu

";

            Console.WriteLine(attachmentMenuText);

            int selectedOption = 0;
            do
            {
                Console.WriteLine("Please select an option and press ENTER:");

                try
                {
                    selectedOption = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    selectedOption = 0;
                }
            } while (!Enumerable.Range(1, 3).Contains(selectedOption));

            switch (selectedOption)
            {
                case 1:
                {
                    await _attachmentService.ViewAttachment(selectedAttachment);
                    await showUnfiledAttachments();
                    break;
                    }

                case 2:
                    {
                        await assignToTransaction(selectedAttachment);
                        break;
                    }

                case 3:
                {
                    break;
                }

                case 4:
                    {
                        await AttachmentInboxMenu.Show();
                        break;
                    }
            }
        }

        private static async Task<AttachmentModel> selectAttachment(List<AttachmentModel> attachments)
        {
            Console.WriteLine(attachments.ToStringTable(new[] { "Index", "File Name", "Type" }, a => a.Index, a => a.FileName, a => a.Type));

            int selectedAttachment = 0;

            do
            {
                Console.WriteLine("Please enter an attachment index or [C]ancel and press ENTER:");

                var selectedOption = Console.ReadLine();

                if (selectedOption.ToLower() == "c")
                {
                    await AttachmentInboxMenu.Show();
                }

                try
                {
                    selectedAttachment = Convert.ToInt32(selectedOption);
                }
                catch (Exception)
                {
                    selectedAttachment = 0;
                }
            } while (!Enumerable.Range(1, attachments.Max(x => x.Index)).Contains(selectedAttachment));

            return attachments.FirstOrDefault(x => x.Index == selectedAttachment);
        }

        private static async Task assignToTransaction(AttachmentModel selectedAttachment)
        {
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
                        await _attachmentService.AssignAttachmentToTransaction(selectedAttachment.Id,
                            selectedTransaction.Id);
                        break;
                    }

                case 2:
                    {
                        var selectedTransaction = await TransactionMenu.GetTransactionByPayee();
                        await _attachmentService.AssignAttachmentToTransaction(selectedTransaction.Id,
                            selectedTransaction.Id);
                        break;
                    }

                case 3:
                    {
                        var selectedTransaction = await TransactionMenu.GetTransactionByDate();
                        await _attachmentService.AssignAttachmentToTransaction(selectedTransaction.Id,
                            selectedTransaction.Id);
                        break;
                    }

                case 4:
                    {
                        await LocalAttachmentMenu.Show();
                        break;
                    }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Attachment {selectedAttachment.Id} assigned successfully!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static async Task deleteAttachment(AttachmentModel selectedAttachment)
        {
        }
    }
}