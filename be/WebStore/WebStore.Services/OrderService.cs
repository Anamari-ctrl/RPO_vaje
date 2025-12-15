using WebStore.Entities.Models;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.OrderDTO;
using WebStore.Services.Helpers;

namespace WebStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderResponse> CreateItem(OrderAddRequest? addRequest, string createdBy)
        {
            ArgumentNullException.ThrowIfNull(addRequest);

            ValidationHelper.ModelValidation(addRequest);

            Order order = addRequest.ToOrder();
            order.CreatedBy = createdBy;
            order.Created = DateTime.Now;

            await _repository.AddAsync(order);

            return order.ToOrderResponse();
        }

        public async Task<bool> Deactivate(Guid? itemId, string updatedBy)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Order? order = await _repository.GetByIdAsync(itemId.Value);

            if (order == null)
            {
                return false;
            }

            order.IsActive = false;

            await _repository.UpdateAsync(order);

            return true;
        }

        public async Task<bool> DeleteItem(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Order? order = await _repository.GetByIdAsync(itemId.Value);

            if (order == null)
            {
                return false;
            }

            return await _repository.DeleteAsync(itemId.Value);
        }

        public Task<List<OrderResponse>> GetActiveItems()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderResponse>> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderResponse?> GetItemById(Guid? itemId)
        {
            if (!itemId.HasValue)
            {
                return null;
            }

            Order? order = await _repository.GetByIdAsync(itemId.Value);

            return order?.ToOrderResponse();
        }

        public async Task<List<OrderResponse>> GetUserOrderHistory(Guid? userId)
        {
            if (!userId.HasValue)
            {
                return [];
            }

            List<Order> orders = await _repository.GetUserOrderHistory(userId.Value);

            return orders.Select(x => x.ToOrderResponse()).ToList();
        }

        public async Task<OrderResponse> UpdateItem(OrderUpdateRequest? updateRequest, string updatedBy)
        {
            ArgumentNullException.ThrowIfNull(updateRequest);

            ValidationHelper.ModelValidation(updateRequest);

            Order? order = await _repository.GetByIdAsync(updateRequest.OrderId);

            if (order == null)
            {
                throw new ArgumentException($"Given order with id {updateRequest.OrderId} doesn't exists!");
            }

            await _repository.UpdateAsync(order);

            return order.ToOrderResponse();
        }
    }
}
