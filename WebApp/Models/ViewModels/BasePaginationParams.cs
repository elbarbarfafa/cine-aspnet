namespace WebApp.Models.ViewModels
{
    public class BasePaginationParams(int? pageIndex = 1, int? pageSize = 10) : IPaginationParams
    {
        public int PageIndex { get; set; } = pageIndex == null || pageIndex < 1 ? 1 : pageIndex.Value;
        public int PageSize { get; set; } = pageSize == null || pageSize < 1 ? 10 : pageSize.Value;
    }
}
