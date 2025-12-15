using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.ProductDTO;

namespace WebStore.API.Endpoints.v1
{
    public static class BooksEndpoint
    {
        public static void MapBooksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/books", GetBooks);
            app.MapGet("api/v1/books/", GetBookData);
        }

        public static async Task<IResult> GetBooks(RequestParameters parameters,
                                                   IProductService productService,
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

        public static async Task<IResult> GetBookData(Guid? productId,
                                                      IProductService productService)
        {
            if (!productId.HasValue)
            {
                return Results.Problem("No product id was provided!");
            }

            ProductResponse? productResponse = await productService.GetItemById(productId.Value);

            if (productResponse == null)
            {
                return Results.NotFound("Product was not found!");
            }

            return Results.Ok(productResponse);
        }
    }
}
