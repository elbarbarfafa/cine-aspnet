using Microsoft.Playwright;
using E2ETests.Infrastructure;

namespace E2ETests.Tests
{
    /// <summary>
    /// Tests end-to-end pour l'authentification dans l'application CinéAsp.
    /// Teste les fonctionnalités essentielles basées sur la structure HTML réelle des formulaires.
    /// </summary>
    public class AuthenticationTests : BaseE2ETest
    {
        [Fact]
        public async Task LoginPage_LoadsCorrectly()
        {
            // Act
            await NavigateToAsync("/Identity/Account/Login");

            // Assert - Vérifier le titre et la structure de la page
            await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex(".*Connexion.*"));
            await Expect(Page.Locator("h1:has-text('Connexion')")).ToBeVisibleAsync();
            
            // Vérifier la présence du formulaire avec l'ID spécifique
            var loginForm = Page.Locator("form#account[method='post']");
            await Expect(loginForm).ToBeVisibleAsync();
            
            // Vérifier les champs avec leurs labels spécifiques (form-floating)
            await Expect(Page.Locator(".form-floating input[type='email']")).ToBeVisibleAsync();
            await Expect(Page.Locator("label:has-text('Email')")).ToBeVisibleAsync();
            await Expect(Page.Locator(".form-floating input[type='password']")).ToBeVisibleAsync();
            await Expect(Page.Locator("label:has-text('Password')")).ToBeVisibleAsync();
            
            // Vérifier la checkbox "Remember Me"
            await Expect(Page.Locator("input.form-check-input[type='checkbox']")).ToBeVisibleAsync();
            
            // Vérifier le bouton de connexion
            await Expect(Page.Locator("button#login-submit.btn.btn-primary:has-text('Log in')")).ToBeVisibleAsync();
            
            // Vérifier le lien "Mot de passe oublié"
            await Expect(Page.Locator("a#forgot-password:has-text('Mot de passe oublié?')")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task LoginPage_EmptyForm_ShowsValidationErrors()
        {
            // Arrange
            await NavigateToAsync("/Identity/Account/Login");

            // Act - Soumettre le formulaire vide
            var submitButton = Page.Locator("button#login-submit");
            await submitButton.ClickAsync();
            await WaitForNetworkIdle();

            // Assert - Des erreurs de validation devraient apparaître
            var validationErrors = Page.Locator(".text-danger");
            var errorCount = await validationErrors.CountAsync();
            
            // Il devrait y avoir des erreurs pour les champs requis
            Assert.True(errorCount > 0, "Validation errors should be displayed for empty required fields");
            
            // Vérifier la présence d'un résumé de validation
            await Expect(Page.Locator(".text-danger").First).ToBeVisibleAsync();
        }

        [Fact]
        public async Task ForgotPasswordPage_LoadsCorrectly()
        {
            // Act
            await NavigateToAsync("/Identity/Account/ForgotPassword");

            // Assert - Vérifier le titre et la structure
            await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex(".*oublié.*|.*Forgot.*"));
            
            // Vérifier la présence du formulaire
            var form = Page.Locator("form[method='post']");
            await Expect(form).ToBeVisibleAsync();
            
            // Vérifier le champ email
            await Expect(Page.Locator("input[type='email']")).ToBeVisibleAsync();
            await Expect(Page.Locator("button[type='submit']")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task LoginPage_ForgotPasswordLink_NavigatesCorrectly()
        {
            // Arrange
            await NavigateToAsync("/Identity/Account/Login");

            // Act - Cliquer sur le lien "Mot de passe oublié"
            await Page.Locator("a#forgot-password").ClickAsync();
            await WaitForNetworkIdle();

            // Assert - Vérifier la navigation vers la page de récupération
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*ForgotPassword.*"));
        }
    }
}