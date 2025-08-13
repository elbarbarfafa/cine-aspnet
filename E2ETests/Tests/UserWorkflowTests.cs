using Microsoft.Playwright;
using E2ETests.Infrastructure;

namespace E2ETests.Tests
{
    /// <summary>
    /// Tests end-to-end pour les workflows utilisateur essentiels dans l'application CinéAsp.
    /// Simule des parcours utilisateur réalistes basés sur la structure HTML réelle.
    /// </summary>
    public class UserWorkflowTests : BaseE2ETest
    {
        [Fact]
        public async Task VisitorWorkflow_BrowseSeancesToday_Complete()
        {
            // Arrange - Un visiteur qui veut voir les séances d'aujourd'hui
            var testName = "visitor_browse_seances";

            // Act & Assert - Workflow complet basé sur la structure HTML réelle
            
            // Étape 1: Arrivée sur la page d'accueil
            await NavigateToAsync("/");
            await Expect(Page.Locator("h1.display-4:has-text('Bienvenue dans votre cinéma')")).ToBeVisibleAsync();

            // Étape 2: Clic sur l'onglet "Séances du jour" avec l'icône spécifique
            await Page.Locator("a:has-text('Séances du jour'):has(.fas.fa-calendar-day)").ClickAsync();
            await WaitForNetworkIdle();
            await Expect(Page.Locator("a.nav-link.active:has-text('Séances du jour')")).ToBeVisibleAsync();
            await Expect(Page.Locator(".alert.alert-info:has(.fas.fa-calendar-day)")).ToBeVisibleAsync();

            // Étape 3: Si des cinémas avec séances sont affichés, cliquer sur l'un d'eux
            var cinemaCards = Page.Locator(".card .card-header.bg-info:has(.fas.fa-building)");
            var hasCinemas = await cinemaCards.CountAsync() > 0;
            
            if (hasCinemas)
            {
                // Vérifier la structure des informations de séance
                await Expect(Page.Locator("h6:has(.fas.fa-film)")).ToBeVisibleAsync();
            }

            // Vérifier que l'utilisateur peut naviguer dans l'application
            var navbar = Page.Locator(".navbar");
            await Expect(navbar).ToBeVisibleAsync();
            
            Assert.True(true, "Visitor workflow completed successfully");
        }

        [Fact]
        public async Task VisitorWorkflow_SearchFilms_Complete()
        {
            // Arrange - Un visiteur qui recherche des films
            var testName = "visitor_search_films";

            // Act & Assert - Workflow de recherche de films basé sur la structure HTML réelle
            
            // Étape 1: Page d'accueil
            await NavigateToAsync("/");
            await Expect(Page.Locator("h1.display-4")).ToBeVisibleAsync();

            // Étape 2: Clic sur l'onglet "Rechercher un film"
            await Page.Locator("a:has-text('Rechercher un film'):has(.fas.fa-search)").ClickAsync();
            await WaitForNetworkIdle();
            await Expect(Page.Locator("a.nav-link.active:has-text('Rechercher un film')")).ToBeVisibleAsync();

            // Étape 3: Saisir un terme de recherche dans le champ spécifique
            var searchInput = Page.Locator("input[name='searchFilm']");
            await Expect(searchInput).ToBeVisibleAsync();
            await searchInput.FillAsync("Action");
            
            // Cliquer sur le bouton de recherche avec l'icône
            await Page.Locator("button[type='submit']:has(.fas.fa-search)").ClickAsync();
            await WaitForNetworkIdle();

            // Vérifier que l'URL contient les bons paramètres
            var finalUrl = Page.Url;
            Assert.Contains("searchType=film", finalUrl);
            Assert.Contains("searchFilm=Action", finalUrl);
            
            // Vérifier que la recherche est bien affichée dans le champ
            await Expect(searchInput).ToHaveValueAsync("Action");
        }

        [Fact]
        public async Task VisitorWorkflow_SearchCinemas_Complete()
        {
            // Arrange - Un visiteur qui recherche des cinémas
            var testName = "visitor_search_cinemas";

            // Act & Assert - Workflow de recherche de cinémas basé sur la structure HTML réelle
            
            // Étape 1: Accès à la recherche de cinémas via l'onglet spécifique
            await NavigateToAsync("/");
            await Page.Locator("a:has-text('Rechercher un cinéma'):has(.fas.fa-building)").ClickAsync();
            await WaitForNetworkIdle();
            await Expect(Page.Locator("a.nav-link.active:has-text('Rechercher un cinéma')")).ToBeVisibleAsync();

            // Étape 2: Recherche avec le formulaire spécifique
            var searchInput = Page.Locator("input[name='searchCinema']");
            await Expect(searchInput).ToBeVisibleAsync();
            await searchInput.FillAsync("Pathé");
            
            await Page.Locator("button[type='submit']:has(.fas.fa-search)").ClickAsync();
            await WaitForNetworkIdle();

            // Étape 3: Vérifier les résultats ou les messages d'absence de résultats
            var cinemaCards = Page.Locator(".cinema-card");
            var emptyMessage = Page.Locator(".text-center:has(.fas.fa-building.fa-3x)");
            
            var hasCinemas = await cinemaCards.CountAsync() > 0;
            var hasEmptyMessage = await emptyMessage.CountAsync() > 0;
            
            if (hasCinemas)
            {
                // Vérifier la structure des cartes de cinéma
                await Expect(cinemaCards.First.Locator(".card-header.bg-primary:has(.fas.fa-building)")).ToBeVisibleAsync();
            }
            else if (hasEmptyMessage)
            {
                await Expect(Page.GetByText("Aucun cinéma trouvé")).ToBeVisibleAsync();
            }

            // Vérifier la cohérence de l'interface
            await Expect(Page.Locator("body")).ToBeVisibleAsync();
            await Expect(Page.Locator(".navbar-brand:has(.fas.fa-film)")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task VisitorWorkflow_NavigationBetweenTabs_Seamless()
        {
            // Arrange - Test de navigation fluide entre tous les onglets
            var testName = "visitor_navigation";

            // Act & Assert - Navigation complète entre les onglets
            await NavigateToAsync("/");

            // Navigation: Cinémas -> Films -> Séances du jour
            var tabs = new (string tabSelector, string expectedParam)[]
            {
                ("a:has-text('Rechercher un cinéma'):has(.fas.fa-building)", "searchType=cinema"),
                ("a:has-text('Rechercher un film'):has(.fas.fa-search)", "searchType=film"),
                ("a:has-text('Séances du jour'):has(.fas.fa-calendar-day)", "searchType=today")
            };

            foreach (var (tabSelector, expectedParam) in tabs)
            {
                await Page.Locator(tabSelector).ClickAsync();
                await WaitForNetworkIdle();
                
                // Vérifier que l'onglet est actif
                await Expect(Page.Locator(".nav-link.active")).ToBeVisibleAsync();
                
                // Vérifier l'URL
                var currentUrl = Page.Url;
                Assert.Contains(expectedParam, currentUrl);
            }

            Assert.True(true, "Navigation workflow completed successfully");
        }
    }
}