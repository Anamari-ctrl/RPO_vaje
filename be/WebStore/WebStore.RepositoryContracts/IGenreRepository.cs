using WebStore.Entities.Models;

namespace WebStore.RepositoryContracts
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetGenres();
    }
}
