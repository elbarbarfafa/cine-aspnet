using Microsoft.Playwright;
using E2ETests.Infrastructure;

namespace E2ETests.Tests
{
    /// <summary>
    /// Tests end-to-end pour la page d'accueil de l'application CinéAsp.
    /// Teste la navigation et les fonctionnalités principales de recherche basées sur la structure HTML réelle.
    /// </summary>
    public class HomePageTests : BaseE2ETest
    {
        [Fact]
        public async Task HomePage_LoadsSuccessfully()
        {
            // Act
            await NavigateToAsync("/");

            // Assert - Vérifier que la page a le bon titre
            await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex(".*Cinéma.*Accueil.*"));
            
            // Vérifier la présence du titre principal avec icône
            await Expect(Page.Locator("h1.display-4:has-text('Bienvenue dans votre cinéma')")).ToBeVisibleAsync();
            
            // Vérifier la présence de l'icône de film
            await Expect(Page.Locator(".fas.fa-film").First).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HomePage_ContainsNavigationTabs()
        {
            // Act
            await NavigateToAsync("/");

            // Assert - Vérifier les onglets de navigation spécifiques
            await Expect(Page.GetByRole(AriaRole.Tab)).ToHaveCountAsync(3);
            
            // Vérifier chaque onglet avec ses icônes spécifiques
            await Expect(Page.Locator("a:has-text('Rechercher un cinéma'):has(.fas.fa-building)")).ToBeVisibleAsync();
            await Expect(Page.Locator("a:has-text('Rechercher un film'):has(.fas.fa-search)")).ToBeVisibleAsync();
            await Expect(Page.Locator("a:has-text('Séances du jour'):has(.fas.fa-calendar-day)")).ToBeVisibleAsync();
            
            // Vérifier la présence du footer
            var footer = Page.Locator(".footer");
            await Expect(footer).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HomePage_SearchCinema_WorksCorrectly()
        {
            // Arrange
            await NavigateToAsync("/?searchType=cinema");

            // Assert - Vérifier que l'onglet cinéma est actif
            await Expect(Page.Locator("a.nav-link.active:has-text('Rechercher un cinéma')")).ToBeVisibleAsync();

            // Act - Rechercher un cinéma avec le formulaire spécifique
            var searchInput = Page.Locator("input[name='searchCinema']");
            await Expect(searchInput).ToBeVisibleAsync();
            
            await searchInput.FillAsync("Pathé");
            
            // Chercher le bouton avec l'icône de recherche
            var submitButton = Page.Locator("button[type='submit']:has(.fas.fa-search)");
            await submitButton.ClickAsync();
            await WaitForNetworkIdle();

            // Assert - Vérifier que la recherche a été effectuée
            await Expect(Page.Locator("input[name='searchCinema']")).ToHaveValueAsync("Pathé");
        }

        [Fact]
        public async Task HomePage_SearchFilm_WorksCorrectly()
        {
            // Arrange
            await NavigateToAsync("/?searchType=film");

            // Assert - Vérifier que l'onglet film est actif
            await Expect(Page.Locator("a.nav-link.active:has-text('Rechercher un film')")).ToBeVisibleAsync();
            
            // Vérifier la présence du formulaire de recherche de film
            await Expect(Page.Locator("input[name='searchFilm']")).ToBeVisibleAsync();
            await Expect(Page.Locator("button[type='submit']:has(.fas.fa-search)")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HomePage_TodaySeances_WorksCorrectly()
        {
            // Arrange
            await NavigateToAsync("/?searchType=today");

            // Assert - Vérifier que l'onglet séances du jour est actif
            await Expect(Page.Locator("a.nav-link.active:has-text('Séances du jour')")).ToBeVisibleAsync();
            
            // Vérifier la présence de l'alerte d'information avec la date
            await Expect(Page.Locator(".alert.alert-info:has(.fas.fa-calendar-day)")).ToBeVisibleAsync();
            await Expect(Page.Locator("h6.alert-heading:has-text('Séances du')")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HomePage_NavigationBetweenTabs_Works()
        {
            // Arrange
            await NavigateToAsync("/");

            // Act & Assert - Tester la navigation entre les onglets
            
            // Cliquer sur l'onglet cinéma
            await Page.Locator("a:has-text('Rechercher un cinéma')").ClickAsync();
            await WaitForNetworkIdle();
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*searchType=cinema.*"));
            
            // Cliquer sur l'onglet film
            await Page.Locator("a:has-text('Rechercher un film')").ClickAsync();
            await WaitForNetworkIdle();
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*searchType=film.*"));
            
            // Cliquer sur l'onglet séances du jour
            await Page.Locator("a:has-text('Séances du jour')").ClickAsync();
            await WaitForNetworkIdle();
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*searchType=today.*"));
        }

        [Fact]
        public async Task HomePage_EmptySearchResults_DisplayCorrectMessage()
        {
            // Act - Rechercher un terme qui ne donnera probablement aucun résultat
            await NavigateToAsync("/?searchType=cinema&searchCinema=CinemaInexistant123456");
            await WaitForNetworkIdle();

            // Assert - Vérifier le message d'aucun résultat
            await Expect(Page.Locator(".text-center:has(.fas.fa-building.fa-3x)")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Aucun cinéma trouvé")).ToBeVisibleAsync();
        }
    }
}