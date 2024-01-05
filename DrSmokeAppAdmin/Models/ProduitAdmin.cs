using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrSmokeAppAdmin.Models
{
    public class ProduitAdmin
    {
        public string Uuid { get; set; } = string.Empty;
        public string CategorieProduit { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public string Descriptif { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public bool Q1g { get; set; }
        public bool Q3g { get; set; }
        public bool Q5g { get; set; }
        public bool Q10g { get; set; }
        public bool Q20g { get; set; }
        public decimal UnGprix { get; set; }
        public decimal TroisGprix { get; set; }
        public decimal CingGprix { get; set; }
        public decimal DixGprix { get; set; }
        public decimal VingtGprix { get; set; }
        public string ImgProduit { get; set; } = string.Empty;

    }
}
