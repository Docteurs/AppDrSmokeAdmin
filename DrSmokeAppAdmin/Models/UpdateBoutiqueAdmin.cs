using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrSmokeAppAdmin.Models
{
    public class UpdateBoutiqueAdmin
    {
        public string Email { get; set; } = string.Empty;
        public string Addresse { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public bool LundiOuvert { get; set; }
        public bool MardiOuvert { get; set; }
        public bool MercrediOuvert { get; set; }
        public bool JeudiOuvert { get; set; }
        public bool VendrediOuvert { get; set; }
        public bool SamediOuvert { get; set; }
        public bool DimancheOuvert { get; set; }
        public string HoraireLundi { get; set; } = string.Empty;
        public string HoraireMardi { get; set; } = string.Empty;
        public string HoraireMercredi { get; set; } = string.Empty;
        public string HoraireJeudi { get; set; } = string.Empty;
        public string HoraireVendredi { get; set; } = string.Empty;
        public string HoraireSamedi { get; set; } = string.Empty;
        public string HoraireDimanche { get; set; } = string.Empty;
    }
}
