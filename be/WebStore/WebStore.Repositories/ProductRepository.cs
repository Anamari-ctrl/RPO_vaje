using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
