using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheMonitaur.WebApps.Landing.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotUsernameConfirmation : PageModel
    {
        public void OnGet()
        {
        }
    }
}
