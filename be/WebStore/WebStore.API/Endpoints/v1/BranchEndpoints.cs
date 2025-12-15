using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.BranchDTO;

namespace WebStore.API.Endpoints.v1
{
    public static class BranchEndpoints
    {
        public static void MapBranchEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/branches", GetBranches);
        }

        public static async Task<IResult> GetBranches(IBranchService branchService)
        {
            List<BranchResponse> branches = await branchService.GetActiveItems();

            return Results.Ok(branches);
        }
    }
}
