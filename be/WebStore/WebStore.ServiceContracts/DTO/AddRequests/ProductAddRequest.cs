using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.Requests
{
    public class ProductAddRequest
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }


        public Product ToProduct()
        {
            return new Product()
            {
                ProductName = ProductName,
                Description = Description
            };
        }
    }
}
