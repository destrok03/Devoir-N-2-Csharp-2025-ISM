namespace gestionapp.Models
{
    public class ApprovisionnementLigne
    {
        public int Id { get; set; }

        public int ApprovisionnementId { get; set; }
        public Approvisionnement Approvisionnement { get; set; } = null!;

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        public decimal MontantLigne => Quantite * PrixUnitaire;
    }
}
