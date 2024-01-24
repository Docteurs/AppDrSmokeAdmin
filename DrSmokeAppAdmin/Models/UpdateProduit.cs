using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

using System.Threading.Tasks;

namespace DrSmokeAppAdmin.Models
{
    public class ProduitAdminUpdate
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; } = string.Empty;
        [JsonPropertyName("categorie_produit")]
        public string CategorieProduit { get; set; } = string.Empty;
        [JsonPropertyName("nom_produit")]
        public string NomProduit { get; set; } = string.Empty;
        [JsonPropertyName("descriptif")]
        public string Descriptif { get; set; } = string.Empty;
        [JsonPropertyName("quantite")]
        public int Quantite { get; set; }
  

        [JsonPropertyName("ung_prix")]
        public string UnGprix { get; set; } = string.Empty;

        [JsonPropertyName("troisg_prix")]
        public string TroisGprix { get; set; } = string.Empty;

        [JsonPropertyName("cingg_prix")]
        public string CingGprix { get; set; } = string.Empty;

        [JsonPropertyName("dixg_prix")]
        public string DixGprix { get; set; } = string.Empty;

        [JsonPropertyName("vingtg_prix")]
        public string VingtGprix { get; set; } = string.Empty;


        [JsonPropertyName("img_produit")]
        public string ImgProduit { get; set; } = string.Empty;

        [JsonPropertyName("isVisible")]
        public int IsVisible { get; set; }
    }
}
