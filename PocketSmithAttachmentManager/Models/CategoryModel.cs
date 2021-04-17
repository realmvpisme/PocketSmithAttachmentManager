using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PocketSmithAttachmentManager.Models
{
    public class CategoryModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("colour")]
        public string Colour { get; set; }
        [JsonPropertyName("is_transfer")]
        public bool IsTransfer { get; set; }
        [JsonPropertyName("is_bill")]
        public bool IsBill { get; set; }
        [JsonPropertyName("refund_behaviour")]
        public string RefundBehaviour { get; set; }
        [JsonPropertyName("children")]
        public List<CategoryModel> Children { get; set; }
        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }
        [JsonPropertyName("roll_up")]
        public bool RollUp { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }


    }
}