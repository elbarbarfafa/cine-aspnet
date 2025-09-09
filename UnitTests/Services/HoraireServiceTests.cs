using Moq;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.Services;

namespace UnitTests.Services
{
    public class HoraireServiceTests
    {
        private readonly IHoraireService _horaireService;
        private readonly Mock<IHoraireRepository> _mockRepository;

        public HoraireServiceTests()
        {
            // Utiliser un mock de l'interface du repository
            _mockRepository = new Mock<IHoraireRepository>();
            _horaireService = new HoraireService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllHoraires()
        {
            // Arrange
            var expectedHoraires = new List<Horaire>
            {
                CreateValidHoraire(1, new TimeSpan(14, 0, 0), new TimeSpan(16, 30, 0)),
                CreateValidHoraire(2, new TimeSpan(20, 0, 0), new TimeSpan(22, 30, 0)),
                CreateValidHoraire(3, new TimeSpan(10, 30, 0), new TimeSpan(12, 45, 0))
            };

            _mockRepository.Setup(repo => repo.GetAll())
                          .Returns(expectedHoraires);

            // Act
            var result = _horaireService.GetAll();

            // Assert
            Assert.Equal(expectedHoraires.Count, result.Count);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetOneById_WithValidId_ReturnsCorrectHoraire(int horaireId)
        {
            // Arrange
            var expectedHoraire = CreateValidHoraire(horaireId, new TimeSpan(14, 0, 0), new TimeSpan(16, 30, 0));
            _mockRepository.Setup(repo => repo.GetById(horaireId))
                          .Returns(expectedHoraire);

            // Act
            var result = _horaireService.GetOneById(horaireId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(horaireId, result.Id);
            _mockRepository.Verify(repo => repo.GetById(horaireId), Times.Once);
        }

        [Fact]
        public void GetOneById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            _mockRepository.Setup(repo => repo.GetById(invalidId))
                          .Returns((Horaire?)null);

            // Act
            var result = _horaireService.GetOneById(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetById(invalidId), Times.Once);
        }

        [Fact]
        public void Add_ThrowsNotImplementedException()
        {
            // Arrange
            var horaire = CreateValidHoraire();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _horaireService.Add(horaire));
        }

        [Fact]
        public void Update_ThrowsNotImplementedException()
        {
            // Arrange
            var horaire = CreateValidHoraire();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _horaireService.Update(horaire));
        }

        [Fact]
        public void Delete_ThrowsNotImplementedException()
        {
            // Arrange
            var horaireId = 1;

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _horaireService.Delete(horaireId));
        }

        private Horaire CreateValidHoraire(int id = 1, TimeSpan? heureDebut = null, TimeSpan? heureFin = null)
        {
            return new Horaire
            {
                Id = id,
                HeureDebut = heureDebut ?? new TimeSpan(14, 0, 0),
                HeureFin = heureFin ?? new TimeSpan(16, 30, 0)
            };
        }
    }
}