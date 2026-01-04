using System.ComponentModel.DataAnnotations;
using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.ProductDTO
{
    public class ProductUpdateRequest
    {
        [Required(ErrorMessage = "Product id can't be blank!")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Category id can't be blank!")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Genre id can't be blank!")]
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

        public string? ManufacturerPageUrl { get; set; }

        public string? TechnicalDetails { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public Product ToProduct()
        {
            return new Product()
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
}
