namespace WebStore.ServiceContracts.DTO.ProductDTO
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }

        public string? ProductName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(ProductResponse)) return false;

            ProductResponse? country_to_compare = (ProductResponse)obj;

            return ProductId == country_to_compare.ProductId && ProductName == country_to_compare.ProductName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
