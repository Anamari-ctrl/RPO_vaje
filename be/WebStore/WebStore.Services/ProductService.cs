using WebStore.Entities.Models;
using WebStore.Entities.RequestFeatures;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.OrderItemDTO;
using WebStore.ServiceContracts.DTO.ProductDTO;
using WebStore.Services.Helpers;

namespace WebStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> CreateItem(ProductAddRequest? addRequest)
        {
            ArgumentNullException.ThrowIfNull(addRequest);

            ValidationHelper.ModelValidation(addRequest);

            Product product = addRequest.ToProduct();

            product.ProductId = Guid.NewGuid();
            product.Created = DateTime.Now;

            await _repository.AddAsync(product);

            return product.ToProductResponse();
        }

        public async Task<bool> DeleteItem(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Product? product = await _repository.GetByIdAsync(itemId.Value);

            if (product == null)
            {
                return false;
            }

            return await _repository.DeleteAsync(itemId.Value);
        }

        public Task<List<ProductResponse>> GetActiveItems()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductResponse>> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ProductResponse>> GetAllProductsAsync(RequestParameters parameters,
                                                                          bool onlyActive)
        {
            var productEntities = await _repository.GetAllProductsAsync(parameters,
                                                                        onlyActive);

            var productDTOs = productEntities.Select(x => x.ToProductResponse()).AsEnumerable();

            return PagedList<ProductResponse>.ToPagedList(productEntities, productDTOs);
        }

        public async Task<ProductResponse?> GetItemById(Guid? itemId)
        {
            if (!itemId.HasValue)
            {
                return null;
            }

            Product? product = await _repository.GetByIdAsync(itemId.Value);

            return product?.ToProductResponse();
        }

        public async Task<ProductResponse> UpdateItem(ProductUpdateRequest? updateRequest)
        {
            ArgumentNullException.ThrowIfNull(updateRequest);

            ValidationHelper.ModelValidation(updateRequest);

            Product? product = await _repository.GetByIdAsync(updateRequest.ProductId);

            if (product == null)
            {
                throw new ArgumentException($"Given product with id {updateRequest.ProductId} doesn't exists!");
            }

            await _repository.UpdateAsync(product);

            return product.ToProductResponse();
        }

        public async Task<bool> DecreaseProductStock(List<OrderItemAddRequest> orderItems)
        {
            ArgumentNullException.ThrowIfNull(orderItems);

            List<OrderItem> items = orderItems.Select(x => x.ToOrderItem()).ToList();

            return await _repository.DecreaseProductStock(items);
        }
    }
}
