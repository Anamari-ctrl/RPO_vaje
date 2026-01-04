using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.BranchDTO;
using WebStore.Services.Helpers;

namespace WebStore.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repository;

        public BranchService(IBranchRepository branchRepository)
        {
            _repository = branchRepository;
        }

        public async Task<BranchResponse> CreateItem(BranchAddRequest? addRequest)
        {
            ArgumentNullException.ThrowIfNull(addRequest);

            ValidationHelper.ModelValidation(addRequest);

            Branch branch = addRequest.ToBranch();
            branch.Created = DateTime.Now;

            await _repository.AddAsync(branch);

            return branch.ToBranchResponse();
        }

        public async Task<bool> DeleteItem(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Branch? branch = await _repository.GetByIdAsync(itemId.Value);

            if (branch == null)
            {
                return false;
            }

            return await _repository.DeleteAsync(branch.BranchId);
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

        public async Task<PagedList<BranchResponse>> GetAllBranchesAsync(RequestParameters parameters, bool onlyActive)
        {
            var branches = await _repository.GetAllBranchesAsync(parameters, false);

            var branchesDto = branches.Select(x => x.ToBranchResponse()).AsEnumerable();

            return PagedList<BranchResponse>.ToPagedList(branches, branchesDto);

        }

        public async Task<BranchResponse?> GetItemById(Guid? itemId)
        {
            if (!itemId.HasValue)
            {
                return null;
            }

            Branch? branch = await _repository.GetByIdAsync(itemId.Value);

            return branch?.ToBranchResponse();
        }

        public async Task<BranchResponse> UpdateItem(BranchUpdateRequest? updateRequest)
        {
            ArgumentNullException.ThrowIfNull(updateRequest);

            ValidationHelper.ModelValidation(updateRequest);

            Branch? branch = await _repository.GetByIdAsync(updateRequest.BranchId);

            if (branch == null)
            {
                throw new ArgumentException($"Given branch with id {updateRequest.BranchId} doesn't exists!");
            }

            await _repository.UpdateAsync(branch);

            return branch.ToBranchResponse();
        }
    }
}
