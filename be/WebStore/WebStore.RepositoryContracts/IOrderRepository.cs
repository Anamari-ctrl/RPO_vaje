using WebStore.Entities.Models;

namespace WebStore.RepositoryContracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetUserOrderHistory(Guid userId);
    }
}
