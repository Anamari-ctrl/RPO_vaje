using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts.DTO.ProductDTO;
using WebStore.Services;

namespace WebStore.API.Endpoints.v1
{
    public static class BooksEndpoint
    {
        public static void MapBooksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/books", GetBooks).RequireAuthorization();
        }

        public static async Task<IResult> GetBooks(RequestParameters parameters,
                                                   ProductService productService,
                                                   HttpContext context)
        {
            PagedList<ProductResponse> pagedListBooks = await productService.GetAllProductsAsync(parameters);

            var metaData = new
            {
                pagedListBooks.MetaData.TotalCount,
                pagedListBooks.MetaData.PageSize,
                pagedListBooks.MetaData.CurrentPage,
                pagedListBooks.MetaData.TotalPages,
                pagedListBooks.MetaData.HasNext,
                pagedListBooks.MetaData.HasPrevious
            };

            context.Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metaData));

            return Results.Ok(pagedListBooks);
        }
    }
}
