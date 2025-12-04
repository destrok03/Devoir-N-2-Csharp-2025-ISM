using System.Linq;
using System.Threading.Tasks;
using gestionapp.Models;
using gestionapp.ViewModels;

namespace gestionapp.Services
{
    public class ApprovisionnementService : IApprovisionnementService
    {
        private readonly IApprovisionnementRepository _repo;

        public ApprovisionnementService(IApprovisionnementRepository repo)
        {
            _repo = repo;
        }

        public Task<PagedResult<Approvisionnement>> GetPagedAsync(int page, int pageSize)
            => _repo.GetPagedAsync(page, pageSize);

        public async Task CreateAsync(ApprovisionnementCreateVm vm)
        {
            var appro = new Approvisionnement
            {
                Reference = vm.Reference,
                DateApprovisionnement = vm.DateApprovisionnement,
                FournisseurId = vm.FournisseurId,
                Statut = StatutApprovisionnement.Recu,
                Lignes = vm.Lignes.Select(l => new ApprovisionnementLigne
                {
                    ArticleId = l.ArticleId,
                    Quantite = l.Quantite,
                    PrixUnitaire = l.PrixUnitaire
                }).ToList()
            };

            appro.MontantTotal = appro.Lignes.Sum(l => l.MontantLigne);

            await _repo.AddAsync(appro);
            await _repo.SaveAsync();
        }
    }
}
