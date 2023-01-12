using BasiaProjektRazorPages.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text.RegularExpressions;
using Dapper;

namespace BasiaProjektRazorPages.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string userName { get; set; }
        [BindProperty]
        public string eMail { get; set; }
        [BindProperty]
        public string repeatedPassword { get; set; }
        [BindProperty]
        public string password { get; set; }

        public string alertClass { get; set; } = "alert-warning";
        public string alertValue { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            bool ok = true;
            var mailRegex = new Regex(@"^[^\s@] +@[^\s@] +\.[^\s@]+$"); // ^[^\s@]+@[^\s@]+\.[^\s@]+$
            if (userName.Length < 6)
            {
                ok = false;
                alertClass = "alert-danger";
                alertValue = "Nazwa użytkownika musi mieć conajmniej 6 znaków.";
            }
            if(password.Length < 6)
            {
                ok = false;
                alertClass = "alert-danger";
                if (string.IsNullOrEmpty(alertValue) == false)
                    alertValue += "\n";
                alertValue += "Hasło musi mieć conajmniej 6 znaków.";
            }
            if(string.IsNullOrEmpty(eMail) || mailRegex.IsMatch(eMail) == false)
            {
                ok = false;
                if (string.IsNullOrEmpty(alertValue) == false)
                    alertValue += "\n";
                alertValue = "E-mail jest nieprawidłowy 😡";
            }
            if (ok)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    var result = conn.Query($"INSERT INTO Konto VALUES(NULL, NULL, NULL");
                }
            }
            return Page();
        }
    }
}
