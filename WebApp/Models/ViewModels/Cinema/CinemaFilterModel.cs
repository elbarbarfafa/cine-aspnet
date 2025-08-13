namespace WebApp.Models.ViewModels.Cinema
{
    public class CinemaFilterModel(string? cinemaSearch) : IFilter
    {
        public string? CinemaSearch { get; set; } = cinemaSearch;
    }
}
