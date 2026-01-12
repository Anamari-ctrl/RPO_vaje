using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.GenreDTO
{
    public class GenreResponse
    {
        public Guid GenreId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    public static class GenreResponseExtension
    {
        public static GenreResponse ToGenreResponse(this Genre gengre)
        {
            return new GenreResponse()
            {
                GenreId = gengre.GenreId,
                Name = gengre.Name,
                Description = gengre.Description
            };
        }
    }
}
