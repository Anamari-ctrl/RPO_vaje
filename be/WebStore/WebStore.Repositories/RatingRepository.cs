using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
