using Microsoft.EntityFrameworkCore;
using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Identity;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly DbSet<ApplicationUser> _dbSet;

        public UserRepository(ApplicationDbContext db)
        {
            _dbSet = db.Set<ApplicationUser>();
        }

        public async Task<ApplicationUser?> GetUserData(Guid userId)
        {
            return await _dbSet!.FindAsync(userId);
        }
    }
}
