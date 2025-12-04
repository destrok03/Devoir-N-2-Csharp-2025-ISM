using System.Collections.Generic;
using System.Threading.Tasks;
using gestionapp.Data;
using gestionapp.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionapp.Services
{
    public class FournisseurRepository : IFournisseurRepository
    {
        private readonly AppDbContext _db;

        public FournisseurRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Fournisseur>> GetAllAsync() =>
            _db.Fournisseurs.ToListAsync();
    }
}
