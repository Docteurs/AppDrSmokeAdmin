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
        [JsonPropertyName("horaireLundi")]
        public string HoraireLundi { get; set; } = string.Empty;
        [JsonPropertyName("horaireMardi")]
        public string HoraireMardi { get; set; } = string.Empty;
        [JsonPropertyName("horaireMercredi")]
        public string HoraireMercredi { get; set; } = string.Empty;
        [JsonPropertyName("horaireJeudi")]
        public string HoraireJeudi { get; set; } = string.Empty;
        [JsonPropertyName("horaireVendredi")]
        public string HoraireVendredi { get; set; } = string.Empty;
        [JsonPropertyName("horaireSamedi")]
        public string HoraireSamedi { get; set; } = string.Empty;
        [JsonPropertyName("horaireDimanche")]
        public string HoraireDimanche { get; set; } = string.Empty;
    }
}