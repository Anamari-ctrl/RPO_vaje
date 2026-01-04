using WebStore.Entities.Models;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.RatingDTO;
using WebStore.Services.Helpers;

namespace WebStore.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repository;

        public RatingService(IRatingRepository repository)
        {
            _repository = repository;
        }

        public async Task<RatingResponse> CreateItem(RatingAddRequest? addRequest)
        {
            ArgumentNullException.ThrowIfNull(addRequest);

            ValidationHelper.ModelValidation(addRequest);

            Rating rating = addRequest.ToRating();
            rating.Created = DateTime.Now;

            await _repository.AddAsync(rating);

            return rating.ToRatingResponse();
        }

        public async Task<bool> DeleteItem(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId);

            Rating? order = await _repository.GetByIdAsync(itemId.Value);

            if (order == null)
            {
                return false;
            }

            return await _repository.DeleteAsync(itemId.Value);
        }

        public Task<List<RatingResponse>> GetActiveItems()
        {
            throw new NotImplementedException();
        }

        public Task<List<RatingResponse>> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public async Task<RatingResponse?> GetItemById(Guid? itemId)
        {
            if (!itemId.HasValue)
            {
                return null;
            }

            Rating? rating = await _repository.GetByIdAsync(itemId.Value);

            return rating?.ToRatingResponse();
        }

        public async Task<RatingResponse> UpdateItem(RatingUpdateRequest? updateRequest)
        {
            ArgumentNullException.ThrowIfNull(updateRequest);

            ValidationHelper.ModelValidation(updateRequest);

            Rating? matchingRating = await _repository.GetByIdAsync(updateRequest.RatingId);

            if (matchingRating == null)
            {
                throw new ArgumentException($"Given order with id {updateRequest.RatingId} doesn't exists!");
            }

            matchingRating.RatingValue = updateRequest.RatingValue;
            matchingRating.Comment = updateRequest.Comment;
            matchingRating.Updated = DateTime.Now;

            await _repository.UpdateAsync(matchingRating);

            return matchingRating.ToRatingResponse();
        }
    }
}
