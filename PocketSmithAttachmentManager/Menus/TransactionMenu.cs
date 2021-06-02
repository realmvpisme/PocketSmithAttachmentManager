using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.Models;
using PocketSmithAttachmentManager.Services;
using PocketSmithAttachmentManager.Services.Extensions;

namespace PocketSmithAttachmentManager.Menus
{
    public static class TransactionMenu
    {
        private static readonly TransactionService _transactionService;
        private static readonly AttachmentService _attachmentService;

        static TransactionMenu()
        {
            _transactionService = new TransactionService();
            _attachmentService = new AttachmentService(typeof(TransactionMenu));
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

        public static async Task<TransactionModel> GetTransactionByAmount()
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

            foreach (var transaction in transactions)
            {
                transaction.Attachments = await _attachmentService.GetByTransactionId(transaction.Id);
            }

            return await selectTransaction(transactions);
        }

        public static async Task<TransactionModel> GetTransactionByDate()
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
                catch (Exception)
                {
                    endDate = null;
                }

            } while (endDate == null);

            var transactions = await _transactionService.GetTransactionsByDate(Convert.ToDateTime(startDate), endDate);

            foreach (var transaction in transactions)
            {
                transaction.Attachments = await _attachmentService.GetByTransactionId(transaction.Id);
            }

            return await selectTransaction(transactions);

        }

        public static async Task<TransactionModel> GetTransactionByPayee()
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
   foreach (var transaction in transactions)
            {
                transaction.Attachments = await _attachmentService.GetByTransactionId(transaction.Id);
            }
         

            return await selectTransaction(transactions);
        }



        private static async Task<TransactionModel> selectTransaction(List<TransactionModel> transactions)
        {
            Console.Clear();
            Console.WriteLine(transactions.ToStringTable(new[] { "Index", "Amount", "Date", "Payee", "Type", "Account Name", "Attachments"}, t => t.Index, t => t.Amount, t => t.Date,t => Regex.Match(t.Payee, ".{0,25}").Value, t => t.Type, t => t.TransactionAccount.Name, t => t.Attachments.Count));


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