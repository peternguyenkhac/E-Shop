namespace EShop.Models.ViewModels
{
    public class ProductPagingFilteringViewModel
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public int CategoryId { get; set; } = 0;

        public string SearchString { get; set; } = "";

        public string SortOrder { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;
    }
}
