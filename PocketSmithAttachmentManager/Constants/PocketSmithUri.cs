namespace PocketSmithAttachmentManager.Constants
{
    public static class PocketSmithUri
    {
        public const string GET_ALL_USER_TRANSACTIONS =
            "https://api.pocketsmith.com/v2/users/{userId}/transactions?per_page=100";

        public const string TRANSACTIONS = "https://api.pocketsmith.com/v2/users/{userId}/transactions?per_page=100";
        public const string TRANSACTION_SEARCH =
            "https://api.pocketsmith.com/v2/users/{userId}/transactions?per_page=100&search={searchTerm}";

        public const string TRANSACTIONS_BY_DATE =
            "https://api.pocketsmith.com/v2/users/{userId}/transactions?per_page=100&start_date={startDate}&end_date={endDate}";

        public const string GET_ALL_USER_ATTACHMENTS = "https://api.pocketsmith.com/v2/users/{$UserId}/attachments";

        public const string UPLOAD_ATTACHMENT = "https://api.pocketsmith.com/v2/users/{userId}/attachments";

        public const string ATTACHMENTS_BY_TRANSACTION =
            "https://api.pocketsmith.com/v2/transactions/{transactionId}/attachments";

        public const string UNASSIGNED_ATTACHMENTS =
            "https://api.pocketsmith.com/v2/users/{userId}/attachments?per_page=100&unassigned=1";
    }
}