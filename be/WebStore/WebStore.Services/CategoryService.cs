using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.GenreDTO;

namespace WebStore.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;

        public GenreService(IGenreRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GenreResponse>> GetGenres()
        {
            var genres = await _repository.GetGenres();

            return genres.Select(x => x.ToGenreResponse()).ToList();
        }
    }
}
