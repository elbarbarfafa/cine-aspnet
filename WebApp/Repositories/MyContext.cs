using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    public class MyContext : IdentityDbContext<Admin>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public MyContext() : base() { 
            
        }

        // Ajoutez les DbSet ici
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Horaire> Horaires { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Cinema
            EntityTypeBuilder<Cinema> cinemaEntity = builder.Entity<Cinema>();
            cinemaEntity.HasKey(c => c.Nom);
            cinemaEntity.Property(c => c.Nom).IsRequired().HasMaxLength(100);
            cinemaEntity.Property(c => c.Rue).IsRequired(false).HasMaxLength(500);
            cinemaEntity.Property(c => c.Numero).IsRequired();
            cinemaEntity.HasMany(c => c.Salles).WithOne(s => s.Cinema).HasForeignKey(ol => ol.CinemaNom).OnDelete(DeleteBehavior.Cascade);

            // Salle
            EntityTypeBuilder<Salle> salleEntity = builder.Entity<Salle>();
            salleEntity.HasKey(s => new { s.CinemaNom, s.Numero });
            salleEntity.Property(s => s.Capacite).IsRequired();
            salleEntity.Property(s => s.DateConstruction).IsRequired();
            salleEntity.HasMany(s => s.Seances).WithOne(se => se.Salle);
            salleEntity.HasOne(s => s.Cinema).WithMany(c => c.Salles).HasForeignKey(ol => ol.CinemaNom);

            // Seance
            EntityTypeBuilder<Seance> seanceEntity = builder.Entity<Seance>();
            seanceEntity.HasKey(s => s.Id);
            seanceEntity.Property(s => s.Tarif).IsRequired();
            seanceEntity.Property(s => s.DateSeance).IsRequired();
            seanceEntity.HasOne(s => s.Salle).WithMany(s => s.Seances).IsRequired();
            seanceEntity.HasOne(s => s.Film).WithMany(f => f.Seances).IsRequired();
            seanceEntity.HasOne(s => s.Horaire).WithMany(h => h.Seances).IsRequired();

            // Film
            EntityTypeBuilder<Film> filmEntity = builder.Entity<Film>();
            filmEntity.HasKey(f => f.Id);
            filmEntity.Property(f => f.Titre).IsRequired().HasMaxLength(200);
            filmEntity.Property(f => f.Annee).IsRequired();
            filmEntity.Property(f => f.Genre).IsRequired().HasMaxLength(100);
            filmEntity.HasMany(f => f.Seances).WithOne(s => s.Film);

            // Horaire
            EntityTypeBuilder<Horaire> horaireEntity = builder.Entity<Horaire>();
            horaireEntity.HasKey(h => h.Id);
            horaireEntity.Property(h => h.HeureDebut).HasColumnType("time(0)").IsRequired();
            horaireEntity.Property(h => h.HeureFin).HasColumnType("time(0)").IsRequired();
            horaireEntity.HasMany(h => h.Seances).WithOne(s => s.Horaire);

            base.OnModelCreating(builder);
        }
    }
}

/*
     * Enable-Migrations
     * Add-Migration initialModel
     * Update-Database
    * 
*/