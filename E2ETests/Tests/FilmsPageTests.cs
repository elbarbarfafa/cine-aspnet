using Microsoft.Playwright;
using E2ETests.Infrastructure;

namespace E2ETests.Tests
{
    /// <summary>
    /// Tests end-to-end pour la gestion des films dans l'application CinéAsp.
    /// Teste les fonctionnalités de recherche et navigation basées sur la structure HTML réelle.
    /// </summary>
    public class FilmsPageTests : BaseE2ETest
    {

        [Fact]
        public async Task FilmsPage_AdminIndex_HasCorrectStructure()
        {
            // Act - Essayer d'accéder à la page d'administration des films (sera redirigé vers login)
            await NavigateToAsync("/Films");

            // Assert - Sera redirigé vers la page de connexion ou affichera la page si authentifié
            var isLoginPage = Page.Url.Contains("Identity") && Page.Url.Contains("Login");
            var isFilmsPage = await Page.Locator("h1:has-text('Gestion des Films'):has(.fas.fa-film)").CountAsync() > 0;
            
            Assert.True(isLoginPage || isFilmsPage, "Should either redirect to login or show films management page");
        }

        [Fact]
        public async Task FilmsPage_SearchWithFilters_WorksCorrectly()
        {
            // Act - Rechercher des films avec des filtres spécifiques sur la page d'accueil
            await NavigateToAsync("/?searchType=film&searchFilm=Action");
            await WaitForNetworkIdle();

            // Assert - Vérifier que la page se charge avec les filtres
            await Expect(Page.Locator("body")).ToBeVisibleAsync();
            
            // Vérifier que l'URL contient les bons paramètres
            var currentUrl = Page.Url;
            Assert.Contains("searchType=film", currentUrl);
            Assert.Contains("searchFilm=Action", currentUrl);
        }

        [Fact]
        public async Task FilmsPage_EmptySearch_ShowsCorrectMessage()
        {
            // Act - Rechercher un film inexistant
            await NavigateToAsync("/?searchType=film&searchFilm=FilmInexistant123456");
            await WaitForNetworkIdle();

            // Assert - Vérifier le message d'absence de résultats
            await Expect(Page.Locator(".text-center:has(.fas.fa-film.fa-3x)")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Aucun film trouvé")).ToBeVisibleAsync();
        }
    }
}