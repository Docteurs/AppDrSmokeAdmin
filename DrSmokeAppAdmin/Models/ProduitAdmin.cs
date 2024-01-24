using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

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
        [JsonConverter(typeof(BoolFromTinyIntConverter))]
        [Column("isVisible")]
        public bool IsVisible { get; set; }


    }
    public class BoolFromTinyIntConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                // Handle null values as needed
                return false; // or true, depending on your logic for null values
            }

            if (reader.TryGetInt32(out int value))
            {
                return value == 1;
            }

            return false;
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value ? 1 : 0);
        }
    }

}
