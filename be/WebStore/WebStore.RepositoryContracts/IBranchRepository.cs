using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;

namespace WebStore.RepositoryContracts
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<PagedList<Branch>> GetAllBranchesAsync(RequestParameters parameters, bool onlyActive);
    }
}
