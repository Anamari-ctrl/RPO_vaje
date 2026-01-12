using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.CategoryDTO
{
    public class CategoryResponse
    {
        public Guid CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }
    }

    public static class CategoryResponseExtensions
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                ImageUrl = category.ImageUrl,
            };
        }
    }
}
