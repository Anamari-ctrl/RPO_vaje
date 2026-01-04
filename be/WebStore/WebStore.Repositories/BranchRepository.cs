using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;
using WebStore.Repositories.Extensions;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<PagedList<Branch>> GetAllBranchesAsync(RequestParameters parameters, bool onlyActive)
        {
            var query = _db.Branches!.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var lowerCaseTerm = parameters.SearchTerm.ToLower();
                query = query.Where(x => x.BranchName!.ToLower().Contains(lowerCaseTerm));
            }

            if (onlyActive)
            {
                query = query.Where(x => x.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortColumn))
            {
                query = query.OrderByDynamic(parameters.SortColumn, parameters.SortOrder!);
            }
            else
            {
                query.OrderBy(x => x.BranchName);
            }

            return await PagedList<Branch>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);
        }
    }
}
