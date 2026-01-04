using System.ComponentModel.DataAnnotations;
using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.BranchDTO;
using WebStore.Services.Helpers;

namespace WebStore.API.Endpoints.v1
{
    public static class BranchEndpoints
    {
        public static void MapBranchEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/branches", GetAllActive);
            app.MapPost("api/v1/branches", GetAll);
            app.MapPost("api/v1/products/create", Create).RequireAuthorization("AdminOnly");
            app.MapDelete("api/v1/products/delete", Delete).RequireAuthorization("AdminOnly");
            app.MapGet("api/v1/products/details", Details).RequireAuthorization("AdminOnly");
            app.MapPut("api/v1/products/update", Update).RequireAuthorization("AdminOnly");
        }

        public static async Task<IResult> GetAllActive(IBranchService branchService)
        {
            List<BranchResponse> branches = await branchService.GetActiveItems();

            return Results.Ok(branches);
        }

        public static async Task<IResult> GetAll(RequestParameters requestParameters,
                                                 HttpContext context,
                                                 IBranchService branchService)
        {
            PagedList<BranchResponse> branches = await branchService.GetAllBranchesAsync(requestParameters, onlyActive: false);

            var metaData = new
            {
                branches.MetaData.TotalCount,
                branches.MetaData.PageSize,
                branches.MetaData.CurrentPage,
                branches.MetaData.TotalPages,
                branches.MetaData.HasNext,
                branches.MetaData.HasPrevious
            };

            context.Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metaData));

            return Results.Ok(branches);
        }

        public static async Task<IResult> Create(BranchAddRequest? addRequest,
                                                 IBranchService branchService)
        {
            if (addRequest == null)
            {
                return Results.Problem("Branch data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(addRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
                return Results.Problem(errorsMessages);
            }

            BranchResponse? branchResponse = await branchService.CreateItem(addRequest);

            return Results.Ok(branchResponse);
        }

        public static async Task<IResult> Delete(Guid? branchId,
                                                 IBranchService branchService)
        {
            if (!branchId.HasValue)
            {
                return Results.Problem("Branch id was not provided");
            }

            BranchResponse? branchResponse = await branchService.GetItemById(branchId.Value);

            if (branchResponse == null)
            {
                return Results.Problem($"Product with provided id ({branchId.Value}) was not found!");
            }

            bool result = await branchService.DeleteItem(branchId.Value);

            if (!result)
            {
                return Results.Ok("Error while deleting product. Check backend logs!");
            }

            return Results.Ok("Product was successfuly deleted");
        }

        public static async Task<IResult> Details(Guid? branchId,
                                                  IBranchService branchService)
        {
            if (!branchId.HasValue)
            {
                return Results.Problem("Branch id was not provided!");
            }

            BranchResponse? branchResponse = await branchService.GetItemById(branchId.Value);

            if (branchResponse == null)
            {
                return Results.Problem($"Branch with provided id ({branchId.Value}) was not found!");
            }

            return Results.Ok(branchResponse);
        }

        public static async Task<IResult> Update(BranchUpdateRequest? updateRequest,
                                                 IBranchService branchService)
        {
            if (updateRequest == null)
            {
                return Results.Problem("Branch data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(updateRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));

                return Results.Problem(errorsMessages);
            }

            BranchResponse? branchResponse = await branchService.UpdateItem(updateRequest);

            if (branchResponse == null)
            {
                return Results.Problem("There was an error updating product. Check backend logs!");
            }

            return Results.Ok(branchResponse);
        }
    }
}
