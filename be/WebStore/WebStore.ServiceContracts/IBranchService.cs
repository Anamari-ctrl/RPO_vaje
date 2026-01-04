using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts.DTO.BranchDTO;

namespace WebStore.ServiceContracts
{
    public interface IBranchService : ICommonService<BranchAddRequest, BranchResponse, BranchUpdateRequest>
    {
        Task<PagedList<BranchResponse>> GetAllBranchesAsync(RequestParameters parameters, bool onlyActive);
    }
}
