using System.ComponentModel.DataAnnotations;
using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.ProductDTO
{
    public class ProductAddRequest
    {
        [Required(ErrorMessage = "Category can't be blank!")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Genre can't be blank!")]
        public Guid GenreId { get; set; }

        [Required(ErrorMessage = "Branch can't be blank!")]
        public Guid BranchId { get; set; }

        [Required(ErrorMessage = "Product name can't be blank!")]
        public string? ProductName { get; set; }

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        [Required(ErrorMessage = "Product price can't be blank!")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public int Warranty { get; set; }

        public string ManufacturerPageUrl { get; set; } = string.Empty;

        public string TechnicalDetails { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
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
                IsActive = IsActive,
                CreatedBy = CreatedBy
            };
        }
    }
}
