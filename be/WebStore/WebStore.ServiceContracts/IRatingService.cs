using WebStore.ServiceContracts.DTO.RatingDTO;

namespace WebStore.ServiceContracts
{
    public interface IRatingService : ICommonService<RatingAddRequest, RatingResponse, RatingUpdateRequest>
    {
    }
}
