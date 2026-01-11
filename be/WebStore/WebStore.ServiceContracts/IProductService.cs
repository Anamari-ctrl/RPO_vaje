using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts.DTO.OrderItemDTO;
using WebStore.ServiceContracts.DTO.ProductDTO;

namespace WebStore.ServiceContracts
{
    public interface IProductService : ICommonService<ProductAddRequest, ProductResponse, ProductUpdateRequest>
    {
        Task<PagedList<ProductResponse>> GetAllProductsAsync(RequestParameters parameters, bool onlyActive);
        Task<bool> DecreaseProductStock(List<OrderItemAddRequest> orderItems);
    }
}
