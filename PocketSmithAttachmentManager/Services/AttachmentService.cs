using PocketSmithAttachmentManager.Constants;
using PocketSmithAttachmentManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.Services
{
    public class AttachmentService
    {
        private readonly HttpClient _httpClient;
        private readonly Type _parentMenuType;
        private readonly RestClient _restClient;

        public AttachmentService(Type parentMenuType)
        {
            _httpClient = new HttpClient();
            _httpClient
                .DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            _httpClient
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            _restClient = new RestClient(PocketSmithUri.UNASSIGNED_ATTACHMENTS);
            _parentMenuType = parentMenuType;
        }

        public async Task UploadAttachment(string filePath, long transactionId)
        {
            var uri = PocketSmithUri.UPLOAD_ATTACHMENT;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            Console.Clear();
            Console.WriteLine("Uploading local attachment...");

            var attachmentString = Convert.ToBase64String(File.ReadAllBytes(filePath));

            var requestBody = new ExpandoObject();
            requestBody.TryAdd("file_name", Path.GetFileName(filePath));
            requestBody.TryAdd("file_data", attachmentString);

            var jsonString = JsonSerializer.Serialize(requestBody);

            var httpResponse = await _httpClient.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Attachment upload failed. Returning to the previous menu...");
                Task.Delay(TimeSpan.FromSeconds(5000));

                await (Task)_parentMenuType.GetMethod("Show").Invoke(null, null);
            }

            var responseAttachment = JsonSerializer.Deserialize<AttachmentModel>(await httpResponse.Content.ReadAsStringAsync());

            await assignAttachment(transactionId, responseAttachment.Id);

            Console.WriteLine("Attachment upload succeeded!");

            await deleteLocalAttachment(filePath);

            Console.WriteLine("Returning to menu...");

            await (Task)_parentMenuType.GetMethod("Show").Invoke(null, null);
        }

        public async Task<List<AttachmentModel>> GetUnAssignedAttachments()
        {
            var uri = PocketSmithUri.UNASSIGNED_ATTACHMENTS;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            var httpResponse = await _restClient.Get(uri);
            var attachments = JsonSerializer.Deserialize<List<AttachmentModel>>(httpResponse);

            return indexAttachments(attachments);
        }

        private async Task assignAttachment(long transactionId, long attachmentId)
        {
            var uri = PocketSmithUri.ATTACHMENTS_BY_TRANSACTION;
            uri = uri.Replace("{transactionId}", transactionId.ToString());

            var requestBody = new ExpandoObject();
            requestBody.TryAdd("attachment_id", attachmentId.ToString());

            var httpResponse = await _httpClient.PostAsync(uri,
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    "Attachment could not be assigned to the selected transaction. Returning to the previous menu...");
                Task.Delay(TimeSpan.FromSeconds(5000));

                await (Task)_parentMenuType.GetMethod("Show").Invoke(null, null);
            }
        }

        private async Task deleteLocalAttachment(string filePath, bool isRetry = false)
        {
            if (!isRetry)
            {
                string selectedOption = null;
                do
                {
                    Console.WriteLine("Do you wish to delete the local file? Enter [Y]es or [N]o and press ENTER:");
                    selectedOption = Console.ReadLine();
                } while (selectedOption.ToLower() != "y" && selectedOption.ToLower() != "n");

                if (selectedOption.ToLower() == "n")
                {
                    return;
                }
            }

            try
            {
                File.Delete(filePath);
                Console.WriteLine($"{Path.GetFileName(filePath)} deleted successfully!");
                Console.WriteLine("Press ENTER to continue:");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"{Path.GetFileName(filePath)} could not be deleted. The following error occured: {e}");
                

                string retryOption = "";
                do
                {
                    Console.WriteLine("Enter [R]etry or [C]ancel and press ENTER:");
                    retryOption = Console.ReadLine();
                } while (retryOption.ToLower() != "r" && retryOption.ToLower() != "c");

                if (retryOption.ToLower() == "c")
                {
                    await (Task)_parentMenuType.GetMethod("Show").Invoke(null, null);
                }

                if (retryOption.ToLower() == "r")
                {
                    await deleteLocalAttachment(filePath, true);
                }
            }
        }

        private List<AttachmentModel> indexAttachments(List<AttachmentModel> attachments)
        {
            int attachmentCount = 1;
            foreach (var attachment in attachments)
            {
                attachment.Index = attachmentCount;
                attachmentCount++;
            }

            return attachments;
        }
    }
}