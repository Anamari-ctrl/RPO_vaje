using Microsoft.EntityFrameworkCore;
using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<List<Order>> GetUserOrderHistory(Guid userId)
        {
            return await _db.Orders!.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
