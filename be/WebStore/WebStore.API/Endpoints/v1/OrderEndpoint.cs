using System.ComponentModel.DataAnnotations;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.OrderDTO;
using WebStore.Services.Helpers;

namespace WebStore.API.Endpoints.v1
{
    public static class OrderEndpoint
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/orders/create", Create);
        }

        public static async Task<IResult> Create(OrderAddRequest? addRequest,
                                                 IOrderService orderService,
                                                 IProductService productService)
        {
            if (addRequest == null)
            {
                return Results.Problem("Order data was not provided!");
            }

            if (!ValidationHelper.IsModelValid(addRequest, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
                return Results.Problem(errorsMessages);
            }

            OrderResponse? orderResponse = await orderService.CreateItem(addRequest);

            if (orderResponse == null)
            {
                return Results.Problem("There was an error saving order! Check in backend logs.");
            }

            bool result = await productService.DecreaseProductStock(addRequest.OrderItems);

            if (result)
            {
                return Results.Ok(orderResponse);
            }

            await orderService.DeleteItem(orderResponse.OrderId);

            return Results.Problem("There was an error decreasing stock! Order was deleted!");
        }
    }
}
