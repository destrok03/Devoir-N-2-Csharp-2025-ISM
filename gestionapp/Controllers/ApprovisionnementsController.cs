using System.Linq;
using System.Threading.Tasks;
using gestionapp.Services;
using gestionapp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestionapp.Controllers
{
    public class ApprovisionnementsController : Controller
    {
        private readonly IApprovisionnementService _service;
        private readonly IFournisseurRepository _fournRepo;
        private readonly IArticleRepository _articleRepo;

        public ApprovisionnementsController(
            IApprovisionnementService service,
            IFournisseurRepository fournRepo,
            IArticleRepository articleRepo)
        {
            _service = service;
            _fournRepo = fournRepo;
            _articleRepo = articleRepo;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, 
            string search = "", int? fournisseur = null, int? article = null, 
            string dateDebut = "", string dateFin = "", string tri = "recent")
        {
            var data = await _service.GetPagedAsync(page, int.MaxValue);
            
            // Convertir en ViewModel en premier
            var items = data.Items.Select(a => new ApprovisionnementListItemVm
            {
                Id = a.Id,
                Reference = a.Reference,
                DateApprovisionnement = a.DateApprovisionnement,
                Fournisseur = a.Fournisseur.Nom,
                NombreArticles = a.Lignes.Count,
                MontantTotal = a.MontantTotal,
                Statut = a.Statut
            }).ToList();
            
            // Appliquer les filtres
            var filteredItems = items.AsEnumerable();
            
            // Filtre par recherche
            if (!string.IsNullOrEmpty(search))
            {
                filteredItems = filteredItems.Where(a => a.Reference.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            
            // Filtre par fournisseur
            if (fournisseur.HasValue)
            {
                var fournisseurName = (await _fournRepo.GetAllAsync()).FirstOrDefault(f => f.Id == fournisseur.Value)?.Nom;
                if (fournisseurName != null)
                {
                    filteredItems = filteredItems.Where(a => a.Fournisseur == fournisseurName);
                }
            }
            
            // Filtre par article (utiliser data.Items pour accéder aux lignes)
            if (article.HasValue)
            {
                var approsWithArticle = data.Items
                    .Where(a => a.Lignes.Any(l => l.ArticleId == article.Value))
                    .Select(a => a.Id)
                    .ToList();
                filteredItems = filteredItems.Where(a => approsWithArticle.Contains(a.Id));
            }
            
            // Filtre par date
            if (!string.IsNullOrEmpty(dateDebut) && DateTime.TryParse(dateDebut, out var debut))
            {
                filteredItems = filteredItems.Where(a => a.DateApprovisionnement.Date >= debut.Date);
            }
            
            if (!string.IsNullOrEmpty(dateFin) && DateTime.TryParse(dateFin, out var fin))
            {
                filteredItems = filteredItems.Where(a => a.DateApprovisionnement.Date <= fin.Date);
            }
            
            // Tri
            filteredItems = tri switch
            {
                "ancien" => filteredItems.OrderBy(a => a.DateApprovisionnement),
                "montant" => filteredItems.OrderByDescending(a => a.MontantTotal),
                _ => filteredItems.OrderByDescending(a => a.DateApprovisionnement)
            };

            var filteredList = filteredItems.ToList();
            
            var vm = new ApprovisionnementIndexVm
            {
                PageResult = new Models.PagedResult<ApprovisionnementListItemVm>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = filteredList.Count,
                    Items = filteredList.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                }
            };

            // Passer les listes pour les filtres
            var fournisseurs = await _fournRepo.GetAllAsync();
            var articles = await _articleRepo.GetAllAsync();
            
            ViewBag.Fournisseurs = fournisseurs
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Nom })
                .ToList();
            ViewBag.Articles = articles
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Libelle })
                .ToList();
            
            ViewBag.SearchTerm = search;
            ViewBag.SelectedFournisseur = fournisseur;
            ViewBag.SelectedArticle = article;
            ViewBag.DateDebut = dateDebut;
            ViewBag.DateFin = dateFin;
            ViewBag.Tri = tri;
            
            // Statistiques
            ViewBag.TotalMontant = filteredList.Sum(a => a.MontantTotal);
            var fournisseurStats = filteredList.GroupBy(a => a.Fournisseur)
                .Select(g => new { Nom = g.Key, Montant = g.Sum(a => a.MontantTotal) })
                .OrderByDescending(x => x.Montant)
                .FirstOrDefault();
            ViewBag.FournisseurPrincipal = fournisseurStats?.Nom ?? "N/A";
            ViewBag.MontantFournisseurPrincipal = fournisseurStats?.Montant ?? 0m;

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ApprovisionnementCreateVm
            {
                DateApprovisionnement = System.DateTime.Today
            };

            vm.Fournisseurs = (await _fournRepo.GetAllAsync())
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Nom });

            vm.Articles = (await _articleRepo.GetAllAsync())
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Libelle });

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApprovisionnementCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Fournisseurs = (await _fournRepo.GetAllAsync())
                    .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Nom });

                vm.Articles = (await _articleRepo.GetAllAsync())
                    .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Libelle });

                return View(vm);
            }

            await _service.CreateAsync(vm);
            return RedirectToAction(nameof(Index));
        }
    }
}
