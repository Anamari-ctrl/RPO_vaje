namespace WebStore.Entities.RequestFeatures
{
    public class RequestParameters
    {
        const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public bool? InStock { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? GenreId { get; set; }
    }
}
