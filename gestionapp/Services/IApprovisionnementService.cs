using System.Threading.Tasks;
using gestionapp.Models;
using gestionapp.ViewModels;

namespace gestionapp.Services
{
    public interface IApprovisionnementService
    {
        Task<PagedResult<Approvisionnement>> GetPagedAsync(int page, int pageSize);
        Task CreateAsync(ApprovisionnementCreateVm vm);
    }
}
