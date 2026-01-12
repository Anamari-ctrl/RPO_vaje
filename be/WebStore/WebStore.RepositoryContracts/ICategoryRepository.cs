using WebStore.Entities.Models;

namespace WebStore.RepositoryContracts
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
    }
}
