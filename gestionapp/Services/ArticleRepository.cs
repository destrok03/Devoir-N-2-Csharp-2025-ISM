using System.Collections.Generic;
using System.Threading.Tasks;
using gestionapp.Data;
using gestionapp.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionapp.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _db;

        public ArticleRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Article>> GetAllAsync() =>
            _db.Articles.ToListAsync();
    }
}
