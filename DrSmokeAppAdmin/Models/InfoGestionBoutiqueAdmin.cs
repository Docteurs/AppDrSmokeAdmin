using System.Text.Json.Serialization;

namespace DrSmokeAppAdmin.Models
{
    public class InfoGestionBoutiqueAdmin
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("nom")]
        public string Nom { get; set; } = string.Empty;

        [JsonPropertyName("prenom")]
        public string Prenom { get; set; } = string.Empty;

        [JsonPropertyName("adresse")]
        public string Adresse { get; set; } = string.Empty;

        [JsonPropertyName("code_postal")]
        public int CodePostal { get; set; }

        [JsonPropertyName("imgUrl")]
        public string ImgUrl { get; set; } = string.Empty;

        [JsonPropertyName("ville")]
        public string Ville { get; set; } = string.Empty;
    }
}