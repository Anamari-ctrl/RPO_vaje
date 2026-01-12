using Microsoft.EntityFrameworkCore;
using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        protected readonly DbSet<Category> _dbSet;

        public CategoryRepository(ApplicationDbContext db)
        {
            _dbSet = db.Set<Category>();
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
