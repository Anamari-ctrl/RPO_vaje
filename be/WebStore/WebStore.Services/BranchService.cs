using WebStore.Entities.Models;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.BranchDTO;

namespace WebStore.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repository;

        public BranchService(IBranchRepository branchRepository)
        {
            _repository = branchRepository;
        }

        public Task<BranchResponse> CreateItem(BranchAddRequest? addRequest, string createdBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Deactivate(Guid? itemId, string updatedBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(Guid? itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BranchResponse>> GetActiveItems()
        {
            List<Branch> branches = await _repository.GetAllAsync();

            return branches.Where(x => x.IsActive).Select(x => x.ToBranchResponse()).ToList();
        }

        public async Task<List<BranchResponse>> GetAllItems()
        {
            List<Branch> branches = await _repository.GetAllAsync();

            return branches.Select(x => x.ToBranchResponse()).ToList();
        }

        public Task<BranchResponse?> GetItemById(Guid? itemId)
        {
            throw new NotImplementedException();
        }

        public Task<BranchResponse> UpdateItem(BranchUpdateRequest? updateRequest, string updatedBy)
        {
            throw new NotImplementedException();
        }
    }
}
