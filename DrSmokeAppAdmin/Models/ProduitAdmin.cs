using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrSmokeAppAdmin.Models
{
    public class ProduitAdmin
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
        [Column("quantite")]
        public int Quantite { get; set; }
        [Column("1g")]
        [JsonPropertyName("1g")]
        public decimal Q1g { get; set; }
        [Column("3g")]
        [JsonPropertyName("3g")]
        public decimal Q3g { get; set; }
        [Column("5g")]
        [JsonPropertyName("5g")]
        public decimal Q5g { get; set; }
        [Column("10g")]
        [JsonPropertyName("10g")]
        public decimal Q10g { get; set; }
        [Column("20g")]
        [JsonPropertyName("20g")]
        public decimal Q20g { get; set; }

        [JsonPropertyName("1g_prix")]
        public string UnGprix { get; set; } = string.Empty;

        [JsonPropertyName("3g_prix")]
        public string TroisGprix { get; set; } = string.Empty;

        [JsonPropertyName("5g_prix")]
        public string CingGprix { get; set; } = string.Empty;

        [JsonPropertyName("10g_prix")]
        public string DixGprix { get; set; } = string.Empty;

        [JsonPropertyName("20g_prix")]
        public string VingtGprix { get; set; } = string.Empty;


        [JsonPropertyName("img_produit")]
        public string ImgProduit { get; set; } = string.Empty;
    }
}
