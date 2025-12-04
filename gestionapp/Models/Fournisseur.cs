using System.Collections.Generic;

namespace gestionapp.Models
{
    public class Fournisseur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;

        public ICollection<Approvisionnement>? Approvisionnements { get; set; }
    }
}
