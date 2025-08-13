namespace WebApp.Models.ViewModels.Seance
{
    public class SeanceFilterModel : IFilter
    {
        public int SalleNumero { get; set; }
        public string? SalleCinemaNom { get; set; }
        public string? FilmTitre { get; set; }
        public DateOnly? DateSeance { get; set; }
        public TimeSpan HeureDebut { get; set; } = TimeSpan.Zero;
        public TimeSpan HeureFin { get; set; } = TimeSpan.Zero;
    }
}
