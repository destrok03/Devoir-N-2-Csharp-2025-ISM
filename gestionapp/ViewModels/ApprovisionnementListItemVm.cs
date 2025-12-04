using System;
using gestionapp.Models;

namespace gestionapp.ViewModels
{
    public class ApprovisionnementListItemVm
    {
        public int Id { get; set; }
        public string Reference { get; set; } = null!;
        public DateTime DateApprovisionnement { get; set; }
        public string Fournisseur { get; set; } = null!;
        public int NombreArticles { get; set; }
        public decimal MontantTotal { get; set; }
        public StatutApprovisionnement Statut { get; set; }
    }
}
