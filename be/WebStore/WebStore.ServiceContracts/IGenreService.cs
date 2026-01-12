using WebStore.ServiceContracts.DTO.GenreDTO;

namespace WebStore.ServiceContracts
{
    public interface IGenreService
    {
        Task<List<GenreResponse>> GetGenres();
    }
}
