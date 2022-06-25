using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ELEMENTs.Environment.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        public void OnGet()
        {
            this.Credential = new Credential { UserName = "Administrator" };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Credential.UserName == "Administrator" && 
                Credential.Password == "Password")
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.Email, "admin@mysite.com")
                };
                var identity = new ClaimsIdentity(claims, "tspCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("tspCookieAuth", claimsPrincipal);

                Response.Redirect("/");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "E-Mail")]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }
    }
}
