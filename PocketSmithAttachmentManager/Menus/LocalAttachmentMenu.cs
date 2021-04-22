using PocketSmithAttachmentManager.Models;
using PocketSmithAttachmentManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.Services.Extensions;

namespace PocketSmithAttachmentManager.Menus
{
    public static class LocalAttachmentMenu
    {
        private static readonly AttachmentService _attachmentService;
        private static readonly TransactionService _transactionService;

        static LocalAttachmentMenu()
        {
            _attachmentService = new AttachmentService(typeof(LocalAttachmentMenu));
            _transactionService = new TransactionService();
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
                        var selectedTransaction = await getTransactionByAmount();
                        await _attachmentService.UploadAttachment(attachmentFilePath, selectedTransaction.Id);
                        break;
                    }

                case 2:
                {
                    var selectedTransaction = await getTransactionByPayee();
                    await _attachmentService.UploadAttachment(attachmentFilePath, selectedTransaction.Id);
                    break;
                }

                case 3:
                {
                    var selectedTransaction = await getTransactionByDate();
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

        private static async Task<TransactionModel> getTransactionByAmount()
        {
            decimal transactionAmount = 0.00m;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter the transaction amount or [C]ancel and press ENTER:");

                try
                {
                    var selectedOption = Console.ReadLine();
                    if (selectedOption.ToLower() == "c")
                    {
                        await LocalAttachmentMenu.Show();
                    }

                    transactionAmount = decimal.Parse(selectedOption);
                }
                catch (Exception)
                {
                    transactionAmount = 0.00m;
                }
            } while (transactionAmount <= 0.00m);

            var transactions = await _transactionService.GetTransactionsByAmount(transactionAmount);

            return await selectTransaction(transactions);
        }

        private static async Task<TransactionModel> getTransactionByPayee()
        {
            string transactionPayee = null;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter the payee name or [C]ancel and press ENTER:");
                transactionPayee = Console.ReadLine();

                if (transactionPayee.ToLower() == "c")
                {
                    await LocalAttachmentMenu.Show();
                }
            } while (string.IsNullOrEmpty(transactionPayee));

            var transactions = await _transactionService.GetTransactionsByPayee(transactionPayee);

            return await selectTransaction(transactions);
        }

        private static async Task<TransactionModel> getTransactionByDate()
        {
            DateTime? startDate = null;
            DateTime? endDate = null;

            do
            {
                Console.Clear();
                Console.WriteLine("Please enter a start date or [C]ancel and press ENTER:");

                try
                {
                    var selectedOption = Console.ReadLine();
                    if (selectedOption.ToLower() == "c")
                    {
                        await LocalAttachmentMenu.Show();
                    }

                    startDate = DateTime.Parse(selectedOption);
                }
                catch (Exception)
                {
                    startDate = null;
                }
            } while (startDate == null);

            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Please enter an end date or [C]ancel and press ENTER:");
                  

                    var selectedOption = Console.ReadLine();
                    if (selectedOption.ToLower() == "c")
                    {
                        await LocalAttachmentMenu.Show();
                    }

                    endDate = string.IsNullOrEmpty(selectedOption) ? startDate : DateTime.Parse(selectedOption);
                }
                catch (Exception )
                {
                    endDate = null;
                }

            } while (endDate == null);

            var transactions = await _transactionService.GetTransactionsByDate(Convert.ToDateTime(startDate), endDate);

            return await selectTransaction(transactions);

        }

        private static async Task<TransactionModel> selectTransaction(List<TransactionModel> transactions)
        {
            Console.Clear();
            Console.WriteLine(transactions.ToStringTable(new[]{"Index", "Amount", "Date", "Payee", "Type"}, t => t.Index, t => t.Amount, t => t.Date, t => Regex.Match(t.Payee, ".{0,25}").Value, t => t.Type));
           

            int selectedTransaction = 0;
            do
            {
                Console.WriteLine("Please enter a transaction number or [C]ancel and press ENTER:");

                try
                {
                    var consoleSelection = Console.ReadLine();

                    if (consoleSelection.ToLower() == "c")
                    {
                        await LocalAttachmentMenu.Show();
                    }

                    selectedTransaction = Convert.ToInt32(consoleSelection);
                }
                catch (Exception)
                {
                    selectedTransaction = 0;
                }
            } while (!Enumerable.Range(1, transactions.Max(x => x.Index)).Contains(selectedTransaction));

            return transactions.FirstOrDefault(x => x.Index == selectedTransaction);
        }
    }
}