using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DrSmokeAppAdmin.Models
{
    public class Commande
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("uuidCommande")]
        public string UuidCommande { get; set; } = string.Empty;

        [JsonPropertyName("uuidClient")]
        public string UuidClient {  get; set; } = string.Empty;

        [JsonPropertyName("uuidMagasin")]
        public string UuidMagasin { get; set; } = string.Empty;

        [JsonPropertyName("uuidProduit")]
        public string UuidProduit { get; set; } = string.Empty;

        [JsonPropertyName("quantite")]
        public string Quantite { get; set; } = string.Empty;

        [JsonPropertyName("prixProduit")]
        public string PrixProduit { get; set; } = string.Empty;

        [JsonPropertyName("prixTotalDesProduit")]
        public string PrixTotalDesProduit { get; set; } = string.Empty;

    }
}
