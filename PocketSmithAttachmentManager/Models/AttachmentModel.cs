using System;
using System.Text.Json.Serialization;

namespace PocketSmithAttachmentManager.Models
{
    public class AttachmentModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("file_name")]
        public string FileName { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
        [JsonPropertyName("content_type_meta")]
        public ContentTypeMetaModel ContentTypeMeta { get; set; }
        [JsonPropertyName("original_url")]
        public string OriginalUrl { get; set; }
        [JsonPropertyName("variants")]
        public VariantsModel Variants { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class ContentTypeMetaModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("extension")]
        public string Extension { get; set; }
    }

    public class VariantsModel
    {
        [JsonPropertyName("thumb_url")]
        public string ThumbUrl { get; set; }
        [JsonPropertyName("large_url")]
        public string LargeUrl { get; set; }
    }
}