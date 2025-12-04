using System.Linq;
using System.Threading.Tasks;
using gestionapp.Data;
using gestionapp.Models;
using Microsoft.EntityFrameworkCore;


namespace gestionapp.Services
{
    public class ApprovisionnementRepository : IApprovisionnementRepository
    {
        private readonly AppDbContext _db;

        public ApprovisionnementRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<Approvisionnement>> GetPagedAsync(int page, int pageSize)
        {
            var query = _db.Approvisionnements
                .Include(a => a.Fournisseur)
                .Include(a => a.Lignes);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.DateApprovisionnement)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Approvisionnement>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task AddAsync(Approvisionnement entity)
        {
            await _db.Approvisionnements.AddAsync(entity);
        }

        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}
