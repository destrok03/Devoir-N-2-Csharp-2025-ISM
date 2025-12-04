using System.ComponentModel.DataAnnotations;

namespace gestionapp.ViewModels
{
    public class ApprovisionnementLigneCreateVm
    {
        [Required]
        public int ArticleId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantite { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal PrixUnitaire { get; set; }
    }
}
