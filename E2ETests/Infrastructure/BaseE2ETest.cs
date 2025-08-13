using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace E2ETests.Infrastructure
{
    /// <summary>
    /// Classe de base pour tous les tests E2E utilisant Playwright.
    /// Simplifie la configuration et fournit les outils nécessaires pour les tests.
    /// </summary>
    public abstract class BaseE2ETest : PageTest
    {
        protected readonly string _baseUrl = "http://localhost:5063"; // URL par défaut de l'app WebApp

        /// <summary>
        /// Attend qu'un élément soit visible sur la page.
        /// </summary>
        protected async Task WaitForElementVisible(string selector, int timeoutMs = 5000)
        {
            await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Navigue vers une URL relative à l'application.
        /// </summary>
        protected async Task NavigateToAsync(string relativePath)
        {
            var url = $"{_baseUrl}{relativePath}";
            await Page.GotoAsync(url);
        }

        /// <summary>
        /// Attend qu'une requête réseau se termine.
        /// Utile pour attendre les appels AJAX.
        /// </summary>
        protected async Task WaitForNetworkIdle(int timeoutMs = 5000)
        {
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, 
                new PageWaitForLoadStateOptions { Timeout = timeoutMs });
        }

        /// <summary>
        /// Configure la taille de l'écran pour les tests responsive.
        /// </summary>
        protected async Task SetMobileViewportAsync()
        {
            await Page.SetViewportSizeAsync(375, 667);
        }

        /// <summary>
        /// Configure la taille de l'écran pour les tests desktop.
        /// </summary>
        protected async Task SetDesktopViewportAsync()
        {
            await Page.SetViewportSizeAsync(1920, 1080);
        }

        /// <summary>
        /// Obtient la taille actuelle du viewport.
        /// </summary>
        protected PageViewportSizeResult? GetViewportSize()
        {
            return Page.ViewportSize;
        }
    }
}