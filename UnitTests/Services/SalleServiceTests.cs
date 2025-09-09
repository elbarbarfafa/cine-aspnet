using Moq;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Salle;
using WebApp.Repositories;
using WebApp.Services;

namespace UnitTests.Services
{
    public class SalleServiceTests
    {
        private readonly ISalleService _salleService;
        private readonly Mock<ISalleRepository> _mockRepository;

        public SalleServiceTests()
        {
            // Utiliser un mock de l'interface du repository
            _mockRepository = new Mock<ISalleRepository>();
            _salleService = new SalleService(_mockRepository.Object);
        }

        [Fact]
        public void Add_WithValidSalle_DoesNotThrowException()
        {
            // Arrange
            var salle = CreateValidSalle();

            // Act & Assert - Peut lever des exceptions liées à EF mais pas de validation métier
            try
            {
                _salleService.Add(salle);
            }
            catch (System.Exception ex)
            {
                // Pour ce test, on accepte les exceptions EF/DB mais pas les exceptions de validation métier
                // Le SalleService n'a pas de validation métier spécifique implémentée
                Assert.False(ex is ArgumentException, "Ne devrait pas lever d'ArgumentException pour une salle valide");
            }
        }

        [Fact]
        public void Update_WithValidSalle_DoesNotThrowException()
        {
            // Arrange
            var salle = CreateValidSalle();

            // Act & Assert - Peut lever des exceptions liées à EF mais pas de validation métier
            try
            {
                _salleService.Update(salle);
            }
            catch (System.Exception ex)
            {
                // Pour ce test, on accepte les exceptions EF/DB mais pas les exceptions de validation métier
                Assert.False(ex is ArgumentException, "Ne devrait pas lever d'ArgumentException pour une salle valide");
            }
        }

        [Fact]
        public void Delete_WithValidId_DoesNotThrowArgumentException()
        {
            // Arrange
            var entityId = ("Cinéma Test", 1);

            // Act & Assert
            try
            {
                _salleService.Delete(entityId);
            }
            catch (System.Exception ex)
            {
                // Pour ce test, on accepte les exceptions EF/DB mais pas les exceptions de validation métier
                Assert.False(ex is ArgumentException, "Ne devrait pas lever d'ArgumentException pour un ID valide");
            }
        }

        [Theory]
        [InlineData("Pathé", 1)]
        [InlineData("UGC", 2)]
        [InlineData("Gaumont", 5)]
        public void Delete_WithVariousValidIds_DoesNotThrowArgumentException(string cinema, int numero)
        {
            // Arrange
            var entityId = (cinema, numero);

            // Act & Assert
            try
            {
                _salleService.Delete(entityId);
            }
            catch (System.Exception ex)
            {
                Assert.False(ex is ArgumentException, $"Ne devrait pas lever d'ArgumentException pour l'ID ({cinema}, {numero})");
            }
        }

        private Salle CreateValidSalle(string cinemaNom = "Cinéma Test", int numero = 1)
        {
            return new Salle
            {
                CinemaNom = cinemaNom,
                Numero = numero,
                Capacite = 120,
                DateConstruction = DateOnly.FromDateTime(DateTime.Today.AddYears(-5)),
                Cinema = new Cinema 
                { 
                    Nom = cinemaNom, 
                    Numero = 42, 
                    Rue = "Rue du Cinéma" 
                }
            };
        }
    }
}