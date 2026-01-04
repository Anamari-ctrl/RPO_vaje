using System.ComponentModel.DataAnnotations;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.RatingDTO;
using WebStore.Services.Helpers;

namespace WebStore.API.Endpoints.v1
{
    public static class RatingEndpoints
    {
        public static void MapRatingEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/ratings/create", Create);
            app.MapPut("api/v1/ratings/update", Update);
            app.MapDelete("api/v1/ratings/delete", Delete);
            app.MapGet("api/v1/ratings/", Details);
        }

        public static async Task<IResult> Create(RatingAddRequest? addRequest,
                                                 IRatingService ratingService)
        {
            if (addRequest == null)
            {
                return Results.Problem("Rating data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(addRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
                return Results.Problem(errorsMessages);
            }

            RatingResponse? ratingResponse = await ratingService.CreateItem(addRequest);


            if (ratingResponse == null)
            {
                return Results.Problem("There was an error creating rating. Check backend logs!");
            }

            return Results.Ok(ratingResponse);
        }

        public static async Task<IResult> Update(RatingUpdateRequest? updateRequest,
                                                 IRatingService ratingService)
        {
            if (updateRequest == null)
            {
                return Results.Problem("Rating data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(updateRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
                return Results.Problem(errorsMessages);
            }

            RatingResponse? ratingResponse = await ratingService.UpdateItem(updateRequest);

            if (ratingResponse == null)
            {
                return Results.Problem("There was an error updating rating. Check backend logs!");
            }

            return Results.Ok(ratingResponse);
        }

        public static async Task<IResult> Delete(Guid? ratingId,
                                                 IRatingService ratingService)
        {
            if (!ratingId.HasValue)
            {
                return Results.Problem("Rating id was not provided!");
            }

            RatingResponse? ratingResponse = await ratingService.GetItemById(ratingId.Value);

            if (ratingResponse == null)
            {
                return Results.Problem($"Rating with provided id ({ratingId}) was not found!");
            }

            bool result = await ratingService.DeleteItem(ratingId.Value);

            if (!result)
            {
                return Results.Ok("Error while deleting rating. Check backend logs!");
            }

            return Results.Ok("Rating was successfuly deleted");
        }

        public static async Task<IResult> Details(Guid? ratingId,
                                                  IRatingService ratingService)
        {
            if (!ratingId.HasValue)
            {
                return Results.Problem("Rating id was not provided!");
            }

            RatingResponse? ratingResponse = await ratingService.GetItemById(ratingId.Value);

            if (ratingResponse == null)
            {
                return Results.Problem($"Rating with provided id ({ratingId}) was not found!");
            }

            return Results.Ok(ratingResponse);
        }
    }
}
