using WebStore.Entities.Models;
using WebStore.ServiceContracts.DTO.RatingDTO;

namespace WebStore.ServiceContracts.DTO.ProductDTO
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid GenreId { get; set; }

        public Guid BranchId { get; set; }

        public int ProductNumber { get; set; }

        public string? ProductName { get; set; }

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public int Warranty { get; set; }

        public string? ManufacturerPageUrl { get; set; }

        public string? TechnicalDetails { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public List<RatingResponse> Ratings { get; set; } = [];

        public ProductUpdateRequest ToProductUpdateRequest()
        {
            return new ProductUpdateRequest()
            {
                ProductId = ProductId,
                CategoryId = CategoryId,
                GenreId = GenreId,
                BranchId = BranchId,
                ProductName = ProductName,
                ShortDescription = ShortDescription,
                LongDescription = LongDescription,
                Price = Price,
                IsAvailable = IsAvailable,
                Warranty = Warranty,
                ManufacturerPageUrl = ManufacturerPageUrl,
                TechnicalDetails = TechnicalDetails,
                ImageUrl = ImageUrl,
                IsActive = IsActive                
            };
        }
    }

    public static class ProductResponseExtensions
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                GenreId = product.GenreId,
                BranchId = product.BranchId,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                Warranty = product.Warranty,
                ManufacturerPageUrl = product.ManufacturerPageUrl,
                TechnicalDetails = product.TechnicalDetails,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                Ratings = product.Ratings.Select(x => x.ToRatingResponse()).ToList()
            };
        }
    }
}
