using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.CategoryDTO;

namespace WebStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryResponse>> GetCategories()
        {
            var categories = await _repository.GetCategories();

            return categories.Select(x => x.ToCategoryResponse()).ToList();
        }
    }
}
