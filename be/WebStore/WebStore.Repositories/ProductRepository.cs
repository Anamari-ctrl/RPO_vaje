using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;
using WebStore.Repositories.Extensions;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<PagedList<Product>> GetAllProductsAsync(RequestParameters request)
        {
            var query = _db.Products!.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var lowerCaseTerm = request.SearchTerm.ToLower();
                query = query.Where(x =>
                x.ProductName!.ToLower().Contains(lowerCaseTerm) ||
                x.LongDescription!.ToLower().Contains(lowerCaseTerm) ||
                x.ShortDescription!.ToLower().Contains(lowerCaseTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                query = query.OrderByDynamic(request.SortColumn, request.SortOrder!);
            }
            else
            {
                query.OrderBy(x => x.ProductName);
            }

            return await PagedList<Product>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
