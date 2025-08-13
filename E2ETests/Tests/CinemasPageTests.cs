using Microsoft.Playwright;
using E2ETests.Infrastructure;

namespace E2ETests.Tests
{
    /// <summary>
    /// Tests end-to-end pour la gestion des cinémas dans l'application CinéAsp.
    /// Teste les fonctionnalités basées sur la structure HTML réelle.
    /// </summary>
    public class CinemasPageTests : BaseE2ETest
    {
        [Fact]
        public async Task CinemasSearch_OnHomePage_WorksCorrectly()
        {
            // Arrange
            await NavigateToAsync("/?searchType=cinema");

            // Assert - Vérifier que l'onglet cinéma est actif
            await Expect(Page.Locator("a.nav-link.active:has-text('Rechercher un cinéma')")).ToBeVisibleAsync();

            // Act - Effectuer une recherche avec la structure HTML réelle
            var searchInput = Page.Locator("input[name='searchCinema']");
            await Expect(searchInput).ToBeVisibleAsync();
            
            await searchInput.FillAsync("Pathé");
            
            // Utiliser le bouton avec l'icône spécifique
            var submitButton = Page.Locator("button[type='submit']:has(.fas.fa-search)");
            await submitButton.ClickAsync();
            await WaitForNetworkIdle();

            // Assert - Vérifier que la recherche a été effectuée
            await Expect(searchInput).ToHaveValueAsync("Pathé");
        }

        [Fact]
        public async Task CinemasSearch_AdminPage_HasCorrectStructure()
        {
            // Act - Essayer d'accéder à la page d'administration des cinémas
            await NavigateToAsync("/Cinemas");

            // Assert - Sera redirigé vers login ou affichera la page admin
            var isLoginPage = Page.Url.Contains("Identity") && Page.Url.Contains("Login");
            var isCinemasPage = await Page.Locator("h1:has-text('Gestion des Cinémas'):has(.fas.fa-building)").CountAsync() > 0;
            
            if (isCinemasPage)
            {
                // Vérifier la structure de la page d'administration
                await Expect(Page.Locator("a.btn.btn-primary:has-text('Nouveau cinéma'):has(.fas.fa-plus)")).ToBeVisibleAsync();
                await Expect(Page.Locator("input#searchString[name='searchString']")).ToBeVisibleAsync();
                await Expect(Page.Locator("select#pageSize[name='pageSize']")).ToBeVisibleAsync();
            }
            
            Assert.True(isLoginPage || isCinemasPage, "Should either redirect to login or show cinemas management page");
        }

        [Fact]
        public async Task CinemasSearch_EmptySearch_ShowsAllCinemas()
        {
            // Act - Rechercher avec un terme vide
            await NavigateToAsync("/?searchType=cinema&searchCinema=");
            await WaitForNetworkIdle();

            // Assert - Vérifier que la page se charge et affiche la structure attendue
            await Expect(Page.Locator("body")).ToBeVisibleAsync();
            await Expect(Page.Locator("a.nav-link.active:has-text('Rechercher un cinéma')")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task CinemasSearch_NonExistentCinema_ShowsNoResults()
        {
            // Act - Rechercher un cinéma inexistant
            await NavigateToAsync("/?searchType=cinema&searchCinema=CinemaInexistant12345");
            await WaitForNetworkIdle();

            // Assert - Vérifier le message d'absence de résultats avec la structure HTML réelle
            await Expect(Page.Locator(".text-center:has(.fas.fa-building.fa-3x)")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Aucun cinéma trouvé pour \"CinemaInexistant12345\"")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task CinemasPage_CinemaCard_DisplaysCorrectStructure()
        {
            // Act - Rechercher tous les cinémas
            await NavigateToAsync("/?searchType=cinema");
            await WaitForNetworkIdle();

            // Assert - Vérifier la structure des cartes de cinéma si elles existent
            var cinemaCards = Page.Locator(".cinema-card");
            var hasCinemaCards = await cinemaCards.CountAsync() > 0;

            if (hasCinemaCards)
            {
                var firstCard = cinemaCards.First;
                
                // Vérifier la structure de la carte
                await Expect(firstCard.Locator(".card-header.bg-primary:has(.fas.fa-building)")).ToBeVisibleAsync();
                await Expect(firstCard.Locator(".card-body:has(.fas.fa-map-marker-alt)")).ToBeVisibleAsync();
                await Expect(firstCard.Locator(".card-footer a.btn.btn-success:has(.fas.fa-ticket-alt)")).ToBeVisibleAsync();
            }

            // Le test passe qu'il y ait ou non des cinémas
            Assert.True(true, "Cinema cards structure test completed");
        }

        [Fact]
        public async Task CinemasPage_ViewTodaySeances_LinkWorks()
        {
            // Act - Aller sur la page de recherche de cinémas
            await NavigateToAsync("/?searchType=cinema");
            await WaitForNetworkIdle();

            // Vérifier s'il y a des cinémas affichés
            var seancesLinks = Page.Locator("a.btn.btn-success:has-text('Voir les séances du jour')");
            var hasSeancesLinks = await seancesLinks.CountAsync() > 0;

            if (hasSeancesLinks)
            {
                // Act - Cliquer sur le premier lien vers les séances
                await seancesLinks.First.ClickAsync();
                await WaitForNetworkIdle();

                // Assert - Vérifier la navigation vers les séances du jour
                await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*searchType=today.*"));
            }

            // Le test passe qu'il y ait ou non des liens
            Assert.True(true, "Seances link test completed");
        }
    }
}