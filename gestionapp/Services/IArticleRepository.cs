using System.Collections.Generic;
using System.Threading.Tasks;
using gestionapp.Models;

namespace gestionapp.Services
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllAsync();
    }
}
