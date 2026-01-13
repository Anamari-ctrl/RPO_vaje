using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.OrderDTO;
using WebStore.Services;

namespace WebStore.Tests.ServiceTests
{
    public class OrderServiceTest
    {
        private readonly IOrderService _orderService;
        private readonly IFixture _fixture;

        private readonly IOrderRepository _orderRepository;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        private readonly IUserRepository _userRepository;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public OrderServiceTest()
        {
            _fixture = new Fixture();

            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderRepository = _orderRepositoryMock.Object;

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = _userRepositoryMock.Object;

            _orderService = new OrderService(_orderRepository, _userRepository);
        }

        //When OrderAddRequest is null, it should return ArgumentNullException
        [Fact]
        public async Task AddOrder_NullOrder_ToBeArgumentNullException()
        {
            //Arrange
            OrderAddRequest? order_add_request = null;

            Order order = _fixture.Build<Order>().With(x => x.OrderItems, null as List<OrderItem>).Create();

            _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Order>())).ReturnsAsync(true);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _orderService.CreateItem(order_add_request);
            });
        }
    }
}
