using System.Text.Json.Serialization;

namespace DrSmokeAppAdmin.Models
{
    public class ReponseAPI
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
