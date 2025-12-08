using WebStore.Entities.RequestFeatures;
using WebStore.ServiceContracts.DTO.ProductDTO;

namespace WebStore.ServiceContracts
{
    public interface IProductService : ICommonService<ProductAddRequest, ProductResponse, ProductResponse>
    {
        Task<PagedList<ProductResponse>> GetAllProductsAsync(RequestParameters parameters);
    }
}
