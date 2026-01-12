using Microsoft.EntityFrameworkCore;
using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;
using WebStore.Repositories.Extensions;
using WebStore.RepositoryContracts;

namespace WebStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<bool> DecreaseProductStock(List<OrderItem> orderItems)
        {
            var productIds = orderItems.Select(i => i.ProductId).ToList();

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var products = await _db.Products!
                                                .AsTracking()
                                                .Where(p => productIds.Contains(p.ProductId))
                                                .ToListAsync();

                foreach (var item in orderItems)
                {
                    var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);

                    if (product == null)
                        throw new Exception($"Product {item.ProductId} not found.");

                    if (product.Stock < item.Quantity)
                        throw new Exception($"Insufficient stock for Product {product.ProductId}.");

                    product.Stock -= item.Quantity;
                }

                int result = await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<PagedList<Product>> GetAllProductsAsync(RequestParameters request, bool onlyActive)
        {
            var query = _db.Products!.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var lowerCaseTerm = request.SearchTerm.ToLower();
                query = query.Where(x =>
                (x.ProductName!.ToLower().Contains(lowerCaseTerm) ||
                x.LongDescription!.ToLower().Contains(lowerCaseTerm) ||
                x.ShortDescription!.ToLower().Contains(lowerCaseTerm)));
            }

            if (onlyActive)
            {
                query = query.Where(x => x.IsActive);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(x => x.Price >= request.MinPrice);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= request.MaxPrice);
            }

            if (request.InStock.HasValue)
            {
                if (request.InStock.Value)
                {
                    query = query.Where(x => x.Stock > 1);
                }
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == request.CategoryId);
            }

            if (request.GenreId.HasValue)
            {
                query = query.Where(x => x.GenreId == request.GenreId);
            }

            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                query = query.OrderByDynamic(request.SortColumn, request.SortOrder!);
            }
            else
            {
                query.OrderBy(x => x.ProductName);
            }

            query = query.Include(x => x.Ratings).AsQueryable();

            return await PagedList<Product>.CreateAsync(query, request.PageNumber, request.PageSize);
        }

        public override async Task<Product?> GetByIdAsync(Guid id)
        {
            Product? product = await _dbSet.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            product.Ratings = await _db.Ratings!.Where(x => x.ProductId == product.ProductId).ToListAsync();

            return product;
        }
    }
}
