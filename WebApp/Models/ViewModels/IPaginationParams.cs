namespace WebApp.Models.ViewModels
{
    public interface IPaginationParams
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
