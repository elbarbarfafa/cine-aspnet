using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    /// <summary>
    /// Représente un cinéma dans le système de gestion.
    /// Un cinéma est défini par un nom unique, une adresse (rue et numéro), et peut contenir plusieurs salles.
    /// Cette entité sert de point d'entrée principal pour l'organisation des séances de films.
    /// </summary>
    public class Cinema
    {
        /// <summary>
        /// Nom unique du cinéma, servant d'identifiant principal.
        /// Utilisé pour identifier de manière unique le cinéma dans le système.
        /// </summary>
        [Required(ErrorMessage = "Le nom du cinéma est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom du cinéma doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom du cinéma")]
        public required string Nom { get; set; }
        
        /// <summary>
        /// Nom de la rue où se situe le cinéma (optionnel).
        /// Complète l'adresse physique du cinéma avec le numéro.
        /// </summary>
        [StringLength(200, ErrorMessage = "Le nom de la rue ne peut pas dépasser 200 caractères")]
        [Display(Name = "Nom de la rue")]
        public string? Rue { get; set; }
        
        /// <summary>
        /// Numéro d'adresse du cinéma.
        /// Obligatoire pour localiser précisément le cinéma, même si le nom de rue n'est pas renseigné.
        /// </summary>
        [Required(ErrorMessage = "Le numéro d'adresse est obligatoire")]
        [Range(1, 9999, ErrorMessage = "Le numéro d'adresse doit être compris entre 1 et 9999")]
        [Display(Name = "Numéro d'adresse")]
        public int Numero { get; set; }
        
        /// <summary>
        /// Collection des salles appartenant à ce cinéma.
        /// Relation un-à-plusieurs : un cinéma peut avoir plusieurs salles.
        /// Initialisée comme liste vide pour éviter les références nulles.
        /// </summary>
        public List<Salle> Salles { get; set; } = [];
    }
}
