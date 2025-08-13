using Moq;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.Services;

namespace UnitTests.Services
{
    public class FilmServiceTests
    {
        private readonly FilmService _filmService;

        public FilmServiceTests()
        {
            // Créer un FilmRepository mocké simple
            var mockRepository = new Mock<FilmRepository>(Mock.Of<MyContext>());
            _filmService = new FilmService(mockRepository.Object);
        }

        [Fact]
        public void Add_FilmWithEmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "",
                Annee = 2023,
                Genre = "Action"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("Le titre du film ne peut pas être vide.", exception.Message);
        }

        [Fact]
        public void Add_FilmWithNullTitle_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = null,
                Annee = 2023,
                Genre = "Action"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("Le titre du film ne peut pas être vide.", exception.Message);
        }

        [Fact]
        public void Add_FilmWithWhitespaceTitle_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "   ",
                Annee = 2023,
                Genre = "Action"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("Le titre du film ne peut pas être vide.", exception.Message);
        }

        [Fact]
        public void Add_FilmWithYearBefore1800_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "Film Ancien",
                Annee = 1799,
                Genre = "Historique"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("L'année du film doit être supérieure à 1800.", exception.Message);
        }

        [Fact]
        public void Add_FilmWithYear1800_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "Film Limite",
                Annee = 1800,
                Genre = "Historique"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("L'année du film doit être supérieure à 1800.", exception.Message);
        }

        [Fact]
        public void Update_FilmWithInvalidTitle_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "",
                Annee = 2023,
                Genre = "Test"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Update(film));
            Assert.Equal("Le titre du film ne peut pas être vide.", exception.Message);
        }

        [Fact]
        public void Update_FilmWithInvalidYear_ThrowsArgumentException()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "Film Valid",
                Annee = 1750,
                Genre = "Test"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Update(film));
            Assert.Equal("L'année du film doit être supérieure à 1800.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Add_FilmWithInvalidTitle_ThrowsArgumentException(string invalidTitle)
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = invalidTitle,
                Annee = 2023,
                Genre = "Action"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("Le titre du film ne peut pas être vide.", exception.Message);
        }

        [Theory]
        [InlineData(1799)]
        [InlineData(1800)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Add_FilmWithInvalidYear_ThrowsArgumentException(int invalidYear)
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Titre = "Film Test",
                Annee = invalidYear,
                Genre = "Action"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _filmService.Add(film));
            Assert.Equal("L'année du film doit être supérieure à 1800.", exception.Message);
        }
    }
}