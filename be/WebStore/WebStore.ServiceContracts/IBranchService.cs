using WebStore.ServiceContracts.DTO.BranchDTO;

namespace WebStore.ServiceContracts
{
    public interface IBranchService : ICommonService<BranchAddRequest, BranchResponse, BranchUpdateRequest>
    {
    }
}
