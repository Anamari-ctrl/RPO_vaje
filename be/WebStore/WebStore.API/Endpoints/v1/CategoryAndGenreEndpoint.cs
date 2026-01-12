using WebStore.ServiceContracts;

namespace WebStore.API.Endpoints.v1
{
    public static class CategoryAndGenreEndpoint
    {
        public static void MapGenreAndCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/genres", GetGenres);
            app.MapGet("api/v1/categories/", GetCategories);
        }

        public static async Task<IResult> GetGenres(IGenreService genreService)
        {
            return Results.Ok(await genreService.GetGenres());
        }

        public static async Task<IResult> GetCategories(ICategoryService categoryService)
        {
            return Results.Ok(await categoryService.GetCategories());
        }
    }
}
