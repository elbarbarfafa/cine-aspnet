// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        /// <summary>
        /// Adresse email pour laquelle la réinitialisation a été demandée
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indique si un utilisateur correspondant à l'email a été trouvé
        /// </summary>
        public bool HasUser { get; set; }

        /// <summary>
        /// Lien de réinitialisation généré (uniquement pour le développement)
        /// </summary>
        public string ResetLink { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public void OnGet(string email = null, bool hasUser = false, string resetLink = null)
        {
            Email = email;
            HasUser = hasUser;
            ResetLink = resetLink;
        }
    }
}
