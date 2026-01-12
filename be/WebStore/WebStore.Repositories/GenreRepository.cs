using Microsoft.EntityFrameworkCore;
using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        protected readonly DbSet<Genre> _dbSet;

        public GenreRepository(ApplicationDbContext db)
        {
            _dbSet = db.Set<Genre>();
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
