namespace WebApp.Models.ViewModels.Salle
{
    public class SalleFilterModel(string cinemaNom, int? numero = null, int? capaciteMin = null, int? capaciteMax = null, DateOnly? dateConstructionDebut = null, DateOnly? dateConstructionFin = null) : IFilter
    {
        public string CinemaNom { get; set; } = cinemaNom;
        public int? Numero { get; set; } = numero;
        public int? CapaciteMin { get; set; } = capaciteMin;
        public int? CapaciteMax { get; set; } = capaciteMax;
        public DateOnly? DateConstructionDebut { get; set; } = dateConstructionDebut;
        public DateOnly? DateConstructionFin { get; set; } = dateConstructionFin;
    }
}
