using WebStore.Entities.Identity;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.OrderDTO;
using WebStore.Services.Helpers;

namespace WebStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository repository,
                            IUserRepository userRepository)
        {
            _orderRepository = repository;
            _userRepository = userRepository;
        }

        public async Task<OrderResponse> CreateItem(OrderAddRequest? addRequest)
        {
            ArgumentNullException.ThrowIfNull(addRequest);

            ValidationHelper.ModelValidation(addRequest);

            Order order = addRequest.ToOrder();
            order.OrderId = Guid.NewGuid();
            order.Created = DateTime.Now;

            if (order.UserId.HasValue)
            {
                ApplicationUser? user = await _userRepository.GetUserData(order.UserId.Value);

                if (user != null)
                {
                    order.CreatedBy = user.FirstName + " " + user.LastName;
                    order.Address = user.Address;
                    order.City = user.City;
                    order.Country = user.Country;
                    order.PostalCode = user.PostalCode;
                }
            }

            await _orderRepository.AddAsync(order);

            return order.ToOrderResponse();
        }

        public async Task<bool> Deactivate(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Order? order = await _orderRepository.GetByIdAsync(itemId.Value);

            if (order == null)
            {
                return false;
            }

            order.IsActive = false;

            await _orderRepository.UpdateAsync(order);

            return true;
        }

        public async Task<bool> DeleteItem(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Order? order = await _orderRepository.GetByIdAsync(itemId.Value);

            if (order == null)
            {
                return false;
            }

            return await _orderRepository.DeleteAsync(itemId.Value);
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

            Order? order = await _orderRepository.GetByIdAsync(itemId.Value);

            return order?.ToOrderResponse();
        }

        public async Task<List<OrderResponse>> GetUserOrderHistory(Guid? userId)
        {
            if (!userId.HasValue)
            {
                return [];
            }

            List<Order> orders = await _orderRepository.GetUserOrderHistory(userId.Value);

            return orders.Select(x => x.ToOrderResponse()).ToList();
        }

        public async Task<OrderResponse> UpdateItem(OrderUpdateRequest? updateRequest)
        {
            ArgumentNullException.ThrowIfNull(updateRequest);

            ValidationHelper.ModelValidation(updateRequest);

            Order? order = await _orderRepository.GetByIdAsync(updateRequest.OrderId);

            if (order == null)
            {
                throw new ArgumentException($"Given order with id {updateRequest.OrderId} doesn't exists!");
            }

            await _orderRepository.UpdateAsync(order);

            return order.ToOrderResponse();
        }
    }
}
