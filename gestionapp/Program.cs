using gestionapp.Data;
using gestionapp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IApprovisionnementRepository, ApprovisionnementRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IFournisseurRepository, FournisseurRepository>();

// Services
builder.Services.AddScoped<IApprovisionnementService, ApprovisionnementService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    
    if (!db.Fournisseurs.Any())
    {
        var fournisseurs = new[]
        {
            new gestionapp.Models.Fournisseur { Nom = "Textiles Dakar SARL" },
            new gestionapp.Models.Fournisseur { Nom = "Mercerie Centrale" },
            new gestionapp.Models.Fournisseur { Nom = "Tissus Premium" },
            new gestionapp.Models.Fournisseur { Nom = "Import Export SA" },
            new gestionapp.Models.Fournisseur { Nom = "Vêtements et Cie" }
        };
        db.Fournisseurs.AddRange(fournisseurs);
        db.SaveChanges();
    }
    
    if (!db.Articles.Any())
    {
        var articles = new[]
        {
            new gestionapp.Models.Article { Libelle = "Coton 100%", PrixUnitaireParDefaut = 5000 },
            new gestionapp.Models.Article { Libelle = "Polyester Premium", PrixUnitaireParDefaut = 3500 },
            new gestionapp.Models.Article { Libelle = "Tissu Wax", PrixUnitaireParDefaut = 4500 },
            new gestionapp.Models.Article { Libelle = "Broderie Traditionnelle", PrixUnitaireParDefaut = 8000 },
            new gestionapp.Models.Article { Libelle = "Fil de couture", PrixUnitaireParDefaut = 1500 },
            new gestionapp.Models.Article { Libelle = "Boutons métalliques", PrixUnitaireParDefaut = 2000 }
        };
        db.Articles.AddRange(articles);
        db.SaveChanges();
    }
    
    if (!db.Approvisionnements.Any())
    {
        var random = new Random();
        var approvisionnements = new List<gestionapp.Models.Approvisionnement>();
        
        // Créer 15 approvisionnements avec dates variées
        var references = new[] { "APP-2023-001", "APP-2023-002", "APP-2023-003", "APP-2023-004", "APP-2023-005",
                                "APP-2023-006", "APP-2023-007", "APP-2023-008", "APP-2023-009", "APP-2023-010",
                                "APP-2023-011", "APP-2023-012", "APP-2023-013", "APP-2023-014", "APP-2023-015" };
        
        var statuts = new[] { 
            gestionapp.Models.StatutApprovisionnement.Recu, 
            gestionapp.Models.StatutApprovisionnement.EnAttente,
            gestionapp.Models.StatutApprovisionnement.Annule
        };
        
        for (int i = 0; i < references.Length; i++)
        {
            var appro = new gestionapp.Models.Approvisionnement
            {
                Reference = references[i],
                DateApprovisionnement = new DateTime(2023, 4, i + 1),
                FournisseurId = (i % 5) + 1,
                Statut = statuts[i % 3],
                Lignes = new List<gestionapp.Models.ApprovisionnementLigne>()
            };
            
            // Ajouter 2-6 lignes par approvisionnement
            int nbLignes = random.Next(2, 7);
            decimal montantTotal = 0;
            
            for (int j = 0; j < nbLignes; j++)
            {
                int articleId = (random.Next(6) + 1);
                int quantite = random.Next(10, 100);
                decimal prix = random.Next(30, 200) * 1000m / 100m;
                
                appro.Lignes.Add(new gestionapp.Models.ApprovisionnementLigne
                {
                    ArticleId = articleId,
                    Quantite = quantite,
                    PrixUnitaire = prix
                });
                montantTotal += quantite * prix;
            }
            
            appro.MontantTotal = montantTotal;
            approvisionnements.Add(appro);
        }
        
        db.Approvisionnements.AddRange(approvisionnements);
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
// applivation 

app.Run();
