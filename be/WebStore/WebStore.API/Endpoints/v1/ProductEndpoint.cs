using System.ComponentModel.DataAnnotations;
using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.ProductDTO;
using WebStore.Services.Helpers;

namespace WebStore.API.Endpoints.v1
{
    public static class ProductEndpoint
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/products", GetAll).RequireAuthorization("AdminOnly");
            app.MapPost("api/v1/products/create", Create).RequireAuthorization("AdminOnly");
            app.MapDelete("api/v1/products/delete", Delete).RequireAuthorization("AdminOnly");
            app.MapGet("api/v1/products/details", Details).RequireAuthorization("AdminOnly");
            app.MapPut("api/v1/products/update", Update).RequireAuthorization("AdminOnly");
        }

        public static async Task<IResult> GetAll(RequestParameters parameters,
                                                 IProductService productService,
                                                 HttpContext context)
        {
            PagedList<ProductResponse> pagedListBooks = await productService.GetAllProductsAsync(parameters,
                                                                                                 onlyActive: false);

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

        public static async Task<IResult> Create(ProductAddRequest? addRequest,
                                                 IProductService productService)
        {
            if (addRequest == null)
            {
                return Results.Problem("Product data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(addRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
                return Results.Problem(errorsMessages);
            }

            ProductResponse? productResponse = await productService.CreateItem(addRequest);

            return Results.Ok(productResponse);
        }
        public static async Task<IResult> Delete(Guid? productId,
                                                 IProductService productService)
        {
            if (!productId.HasValue)
            {
                return Results.Problem("Product id was not provided!");
            }

            ProductResponse? productResponse = await productService.GetItemById(productId.Value);

            if (productResponse == null)
            {
                return Results.Problem($"Product with provided id ({productId.Value}) was not found!");
            }

            bool result = await productService.DeleteItem(productId.Value);

            if (!result)
            {
                return Results.Ok("Error while deleting product. Check backend logs!");
            }

            return Results.Ok("Product was successfuly deleted");
        }

        public static async Task<IResult> Details(Guid? productId,
                                                  IProductService productService)
        {
            if (!productId.HasValue)
            {
                return Results.Problem("Product id was not provided!");
            }

            ProductResponse? productResponse = await productService.GetItemById(productId.Value);

            if (productResponse == null)
            {
                return Results.Problem($"Product with provided id ({productId.Value}) was not found!");
            }

            return Results.Ok(productResponse);
        }


        public static async Task<IResult> Update(ProductUpdateRequest? updateRequest,
                                                 IProductService productService)
        {
            if (updateRequest == null)
            {
                return Results.Problem("Product data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(updateRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));

                return Results.Problem(errorsMessages);
            }

            ProductResponse? productResponse = await productService.UpdateItem(updateRequest);

            if (productResponse == null)
            {
                return Results.Problem("There was an error updating product. Check backend logs!");
            }

            return Results.Ok(productResponse);
        }

    }
}
