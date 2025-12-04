using System.Threading.Tasks;
using gestionapp.Models;

namespace gestionapp.Services
{
    public interface IApprovisionnementRepository
    {
        Task<PagedResult<Approvisionnement>> GetPagedAsync(int page, int pageSize);
        Task AddAsync(Approvisionnement entity);
        Task SaveAsync();
    }
}
