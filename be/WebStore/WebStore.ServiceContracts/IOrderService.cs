using WebStore.ServiceContracts.DTO.OrderDTO;

namespace WebStore.ServiceContracts
{
    public interface IOrderService : ICommonService<OrderAddRequest, OrderResponse, OrderUpdateRequest>
    {
        Task<List<OrderResponse>> GetUserOrderHistory(Guid? userId);
    }
}
