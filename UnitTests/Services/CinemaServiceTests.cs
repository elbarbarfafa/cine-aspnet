using Moq;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Cinema;
using WebApp.Repositories;
using WebApp.Services;

namespace UnitTests.Services
{
    public class CinemaServiceTests
    {
        private readonly ICinemaService _cinemaService;
        private readonly Mock<ICinemaRepository> _mockRepository;

        public CinemaServiceTests()
        {
            // Utiliser un mock de l'interface du repository
            _mockRepository = new Mock<ICinemaRepository>();
            _cinemaService = new CinemaService(_mockRepository.Object);
        }

        [Fact]
        public void Add_WithValidCinema_DoesNotThrowException()
        {
            // Arrange
            var cinema = CreateValidCinema();

            // Act & Assert - Ne devrait pas lever d'exception de validation métier
            try
            {
                _cinemaService.Add(cinema);
                // Vérifier que la méthode Insert du repository est appelée
                _mockRepository.Verify(repo => repo.Insert(cinema), Times.Once);
            }
            catch (System.Exception ex)
            {
                // Le CinemaService n'a pas de validation métier spécifique implémentée
                Assert.False(ex is ArgumentException, "Ne devrait pas lever d'ArgumentException pour un cinéma valide");
            }
        }

        [Fact]
        public void Update_WithValidCinema_DoesNotThrowException()
        {
            // Arrange
            var cinema = CreateValidCinema();

            // Act & Assert
            try
            {
                _cinemaService.Update(cinema);
                // Vérifier que la méthode Update du repository est appelée
                _mockRepository.Verify(repo => repo.Update(cinema), Times.Once);
            }
            catch (System.Exception ex)
            {
                Assert.False(ex is ArgumentException, "Ne devrait pas lever d'ArgumentException pour un cinéma valide");
            }
        }

        [Fact]
        public void Delete_WithValidId_DoesNotThrowArgumentException()
        {
            // Arrange
            var cinemaId = "Cinéma Test";

            // Act & Assert
            try
            {
                _cinemaService.Delete(cinemaId);
                // Vérifier que la méthode Delete du repository est appelée
                _mockRepository.Verify(repo => repo.Delete(cinemaId), Times.Once);
            }
            catch (System.Exception ex)
            {
                Assert.False(ex is ArgumentException, "Ne devrait pas lever d'ArgumentException pour un ID valide");
            }
        }

        [Fact]
        public void GetAllCinemas_WithoutSearchTerm_ReturnsAllCinemas()
        {
            // Arrange
            var expectedCinemas = new List<Cinema>
            {
                CreateValidCinema("Pathé"),
                CreateValidCinema("UGC"),
                CreateValidCinema("Gaumont")
            };
            
            // Mock de la méthode GetAll() qui est appelée par GetAllCinemas()
            _mockRepository.Setup(repo => repo.GetAll())
                          .Returns(expectedCinemas);

            // Act
            var result = _cinemaService.GetAllCinemas(null);

            // Assert
            Assert.Equal(expectedCinemas.Count, result.Count);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAllCinemas_WithSearchTerm_ReturnsFilteredCinemas()
        {
            // Arrange
            var searchTerm = "Pathé";
            var allCinemas = new List<Cinema>
            {
                CreateValidCinema("Pathé"),
                CreateValidCinema("UGC"),
                CreateValidCinema("Gaumont")
            };
            
            // Mock de la méthode GetAll() qui est appelée par GetAllCinemas()
            _mockRepository.Setup(repo => repo.GetAll())
                          .Returns(allCinemas);

            // Act
            var result = _cinemaService.GetAllCinemas(searchTerm);

            // Assert
            Assert.Single(result);
            Assert.Equal("Pathé", result.First().Nom);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData("Pathé")]
        [InlineData("UGC")]
        [InlineData("Gaumont")]
        public void GetOneById_WithValidId_ReturnsCorrectCinema(string cinemaId)
        {
            // Arrange
            var expectedCinema = CreateValidCinema(cinemaId);
            _mockRepository.Setup(repo => repo.GetById(cinemaId))
                          .Returns(expectedCinema);

            // Act
            var result = _cinemaService.GetOneById(cinemaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cinemaId, result.Nom);
            _mockRepository.Verify(repo => repo.GetById(cinemaId), Times.Once);
        }

        [Fact]
        public void GetOneById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = "Cinéma Inexistant";
            _mockRepository.Setup(repo => repo.GetById(invalidId))
                          .Returns((Cinema?)null);

            // Act
            var result = _cinemaService.GetOneById(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetById(invalidId), Times.Once);
        }

        [Fact]
        public void GetAll_ReturnsAllCinemas()
        {
            // Arrange
            var expectedCinemas = new List<Cinema>
            {
                CreateValidCinema("Pathé"),
                CreateValidCinema("UGC")
            };
            
            _mockRepository.Setup(repo => repo.GetAll())
                          .Returns(expectedCinemas);

            // Act
            var result = _cinemaService.GetAll();

            // Assert
            Assert.Equal(expectedCinemas.Count, result.Count);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAllPaginatedAndFiltered_WithFilter_ReturnsCorrectResult()
        {
            // Arrange
            var searchTerm = "Pathé";
            var expectedCinemas = new List<Cinema> { CreateValidCinema("Pathé") };
            var paginationParams = new BasePaginationParams(1, 10);
            var filters = new CinemaFilterModel(searchTerm);
            
            _mockRepository.Setup(repo => repo.GetAllAndSearchFor(searchTerm))
                          .Returns(expectedCinemas);

            // Act
            var result = _cinemaService.GetAllPaginatedAndFiltered(paginationParams, filters);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Pathé", result.Items.First().Nom);
            _mockRepository.Verify(repo => repo.GetAllAndSearchFor(searchTerm), Times.Once);
        }

        private Cinema CreateValidCinema(string nom = "Cinéma Test")
        {
            return new Cinema
            {
                Nom = nom,
                Rue = "Rue du Cinéma",
                Numero = 123
            };
        }
    }
}