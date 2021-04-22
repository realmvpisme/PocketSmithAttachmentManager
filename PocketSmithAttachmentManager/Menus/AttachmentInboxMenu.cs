using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.Models;
using PocketSmithAttachmentManager.Services;
using PocketSmithAttachmentManager.Services.Extensions;

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
    }
}