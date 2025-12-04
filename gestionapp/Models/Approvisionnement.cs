using System;
using System.Collections.Generic;

namespace gestionapp.Models
{
    public class Approvisionnement
    {
        public int Id { get; set; }
        public string Reference { get; set; } = null!;
        public DateTime DateApprovisionnement { get; set; }

        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; } = null!;

        public decimal MontantTotal { get; set; }
        public StatutApprovisionnement Statut { get; set; }

        public ICollection<ApprovisionnementLigne> Lignes { get; set; } =
            new List<ApprovisionnementLigne>();
    }
}
