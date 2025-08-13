using WebApp.Models.Entities;

namespace WebApp.Models.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        // Recherche de cinéma
        public string? SearchCinema { get; set; }
        public List<WebApp.Models.Entities.Cinema> Cinemas { get; set; } = [];

        // Recherche de film
        public string? SearchFilm { get; set; }
        public List<FilmSeanceInfo> FilmSeances { get; set; } = [];

        // Type de recherche active
        public string SearchType { get; set; } = "cinema"; // "cinema", "film", ou "today"

        // Séances du jour
        public List<CinemaSeanceInfo> TodaySeances { get; set; } = [];
    }

    public class FilmSeanceInfo
    {
        public WebApp.Models.Entities.Film Film { get; set; } = null!;
        public List<CinemaSeanceDetail> SeancesByCinema { get; set; } = [];
    }

    public class CinemaSeanceDetail
    {
        public WebApp.Models.Entities.Cinema Cinema { get; set; } = null!;
        public List<WebApp.Models.Entities.Seance> Seances { get; set; } = [];
    }

    public class CinemaSeanceInfo
    {
        public WebApp.Models.Entities.Cinema Cinema { get; set; } = null!;
        public List<FilmSeanceDetail> FilmSeances { get; set; } = [];
    }

    public class FilmSeanceDetail
    {
        public WebApp.Models.Entities.Film Film { get; set; } = null!;
        public List<WebApp.Models.Entities.Seance> Seances { get; set; } = [];
    }
}