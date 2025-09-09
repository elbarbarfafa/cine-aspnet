using WebApp.Models.Entities;
using WebApp.Services;
using Moq;
using WebApp.Repositories;

namespace UnitTests.Services
{
    public class SeanceServiceTests
    {
        private readonly ISeanceService _seanceService;
        private readonly Mock<ISeanceRepository> _mockRepository;

        public SeanceServiceTests()
        {
            // Utiliser un mock de l'interface du repository
            _mockRepository = new Mock<ISeanceRepository>();
            _seanceService = new SeanceService(_mockRepository.Object);
        }

        [Fact]
        public void Add_SeanceWithNegativeTarif_ThrowsArgumentException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.Tarif = -5.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _seanceService.Add(seance));
            Assert.Equal("Le tarif doit être supérieur à 0.", exception.Message);
        }

        [Fact]
        public void Add_SeanceWithZeroTarif_ThrowsArgumentException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.Tarif = 0.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _seanceService.Add(seance));
            Assert.Equal("Le tarif doit être supérieur à 0.", exception.Message);
        }

        [Fact]
        public void Add_SeanceWithPastDate_ThrowsArgumentException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.DateSeance = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _seanceService.Add(seance));
            Assert.Equal("La date de séance ne peut pas être dans le passé.", exception.Message);
        }

        [Fact]
        public void Update_SeanceWithInvalidTarif_ThrowsArgumentException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.Tarif = -10.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _seanceService.Update(seance));
            Assert.Equal("Le tarif doit être supérieur à 0.", exception.Message);
        }

        [Fact]
        public void Update_SeanceWithPastDate_ThrowsArgumentException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.DateSeance = DateOnly.FromDateTime(DateTime.Today.AddDays(-2));

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _seanceService.Update(seance));
            Assert.Equal("La date de séance ne peut pas être dans le passé.", exception.Message);
        }

        [Theory]
        [InlineData(-1.0)]
        [InlineData(0.0)]
        [InlineData(-10.5)]
        public void Add_SeanceWithInvalidTarif_ThrowsArgumentException(double invalidTarif)
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.Tarif = invalidTarif;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _seanceService.Add(seance));
            Assert.Equal("Le tarif doit être supérieur à 0.", exception.Message);
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(10.0)]
        [InlineData(15.50)]
        [InlineData(999.99)]
        public void Add_SeanceWithValidTarif_DoesNotThrowTarifException(double validTarif)
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.Tarif = validTarif;
            seance.DateSeance = DateOnly.FromDateTime(DateTime.Today); // Date valide

            // Act & Assert - Ne devrait pas lever d'exception pour le tarif
            try
            {
                _seanceService.Add(seance);
                // Si on arrive ici, c'est que l'exception de tarif n'a pas été levée (ce qui est bien)
            }
            catch (ArgumentException ex)
            {
                // Vérifier que ce n'est pas l'exception de tarif
                Assert.NotEqual("Le tarif doit être supérieur à 0.", ex.Message);
            }
            catch
            {
                // Autres exceptions possibles (comme des erreurs EF) sont acceptables pour ce test
            }
        }

        [Fact]
        public void Add_SeanceWithTodayDate_DoesNotThrowDateException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.DateSeance = DateOnly.FromDateTime(DateTime.Today);
            seance.Tarif = 10.0; // Tarif valide

            // Act & Assert
            try
            {
                _seanceService.Add(seance);
                // Si on arrive ici, c'est que l'exception de date n'a pas été levée (ce qui est bien)
            }
            catch (ArgumentException ex)
            {
                // Vérifier que ce n'est pas l'exception de date
                Assert.NotEqual("La date de séance ne peut pas être dans le passé.", ex.Message);
            }
            catch
            {
                // Autres exceptions possibles (comme des erreurs EF) sont acceptables pour ce test
            }
        }

        [Fact]
        public void Add_SeanceWithFutureDate_DoesNotThrowDateException()
        {
            // Arrange
            var seance = CreateValidSeance();
            seance.DateSeance = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            seance.Tarif = 10.0; // Tarif valide

            // Act & Assert
            try
            {
                _seanceService.Add(seance);
                // Si on arrive ici, c'est que l'exception de date n'a pas été levée (ce qui est bien)
            }
            catch (ArgumentException ex)
            {
                // Vérifier que ce n'est pas l'exception de date
                Assert.NotEqual("La date de séance ne peut pas être dans le passé.", ex.Message);
            }
            catch
            {
                // Autres exceptions possibles (comme des erreurs EF) sont acceptables pour ce test
            }
        }

        private Seance CreateValidSeance()
        {
            return new Seance
            {
                Id = 1,
                Tarif = 12.50,
                DateSeance = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Film = new Film { Id = 1, Titre = "Film Test", Annee = 2023, Genre = "Action" },
                Salle = new Salle 
                { 
                    CinemaNom = "Cinéma Test", 
                    Numero = 1, 
                    Capacite = 100, 
                    DateConstruction = DateOnly.FromDateTime(DateTime.Today.AddYears(-10)),
                    Cinema = new Cinema { Nom = "Cinéma Test", Numero = 1 }
                },
                Horaire = new Horaire { Id = 1, HeureDebut = new TimeSpan(20, 0, 0), HeureFin = new TimeSpan(22, 30, 0) }
            };
        }
    }
}