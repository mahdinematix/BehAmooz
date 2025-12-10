using System.Text.Json.Serialization;

namespace _01_Framework.Application.TusServices
{
    

    public class FinalizeRequest
    {
        //[JsonPropertyName("Content-Type")]
        //public string ContentType { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("convert_mode")]
        public string ConvertMode { get; set; }

        [JsonPropertyName("file_id")]
        public string FileId { get; set; }
    }

}
