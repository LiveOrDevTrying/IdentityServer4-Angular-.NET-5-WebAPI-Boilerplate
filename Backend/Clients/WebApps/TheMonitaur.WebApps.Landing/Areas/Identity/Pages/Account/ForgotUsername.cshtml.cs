using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TheMonitaur.WebApps.Landing.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotUsername : PageModel
    {
        private readonly UserManager<Domain.CodeFirst.Models.Identity> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotUsername(UserManager<Domain.CodeFirst.Models.Identity> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotUsernameConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Retreive Username",
                    $"Your username is <b>{user.UserName}</b>.");

                return RedirectToPage("./ForgotUsernameConfirmation");
            }

            return Page();
        }
    }
}
