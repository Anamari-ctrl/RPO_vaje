using WebStore.ServiceContracts.DTO.CategoryDTO;

namespace WebStore.ServiceContracts
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetCategories();
    }
}
