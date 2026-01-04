using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;

namespace WebStore.RepositoryContracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<Product>> GetAllProductsAsync(RequestParameters parameters, bool onlyActive);
    }
}
