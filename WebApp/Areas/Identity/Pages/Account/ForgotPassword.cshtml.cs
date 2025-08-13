// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.Models.Entities;

namespace WebApp.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Admin> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Admin> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "L'adresse email est obligatoire")]
            [EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide")]
            [Display(Name = "Adresse email")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Pour des raisons de sécurité, ne révélons pas si l'utilisateur existe ou non
                    // Mais pour le développement, nous allons rediriger vers la page de confirmation sans lien
                    return RedirectToPage("./ForgotPasswordConfirmation", new { email = Input.Email, hasUser = false });
                }

                // Génération du token de réinitialisation
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                // Génération de l'URL de réinitialisation
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code, email = Input.Email },
                    protocol: Request.Scheme);

                // Au lieu d'envoyer un email, nous passons le lien directement à la page de confirmation
                // ATTENTION : Cette approche est uniquement pour le développement !
                return RedirectToPage("./ForgotPasswordConfirmation", new { 
                    email = Input.Email, 
                    hasUser = true, 
                    resetLink = callbackUrl 
                });
            }

            return Page();
        }
    }
}
