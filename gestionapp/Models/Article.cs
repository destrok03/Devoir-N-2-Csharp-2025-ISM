namespace gestionapp.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = null!;
        public decimal PrixUnitaireParDefaut { get; set; }
    }
}
