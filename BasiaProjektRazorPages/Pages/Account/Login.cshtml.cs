using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Dapper;
using BasiaProjektRazorPages.DBModels;
using BasiaProjektRazorPages.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BasiaProjektRazorPages.DbModels;

namespace BasiaProjektRazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string userNameOrMail { get; set; }
        [BindProperty]
        public string password { get; set; }

        public string alertClass { get; set; } = "alert-warning";
        public string alertValue { get; set; }
        public bool loggedIn { get; set; }

        [BindProperty(SupportsGet = true)]
        public string redirectUrl { get; set; }

        public void OnGet()
        {
            var prevPage = Request.Headers["Referer"].ToString();
            if(AccountHelper.checkForLoggedInSession(HttpContext).Item1)
            {
                alertClass = "alert-info";
                alertValue = "Już jesteś zalogowana (sorry for misgender bro)";
            }
        }

        public IActionResult OnPost()
        {
            if (AccountHelper.checkForLoggedInSession(HttpContext).Item1)
            {
                alertClass = "alert-info";
                alertValue = "Już jesteś zalogowana (sorry for misgender bro)";
                return Page();
            }
            bool ok = true;
            // check if empty
            if(string.IsNullOrWhiteSpace(userNameOrMail) || string.IsNullOrWhiteSpace(password))
            {
                alertClass = "alert-danger";
                alertValue = "Człowieku lub Kobieto, żeś nie uzupełnił(a) wszystkich pól.";
                ok = false;
            }
            if (ok)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    // look for account
                    Konto account = null;
                    try
                    {
                        if (AccountHelper.mailRegex.IsMatch(userNameOrMail))
                        {
                            account = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE Email = '{userNameOrMail}'");
                        }
                        else
                        {
                            account = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE LoginUzytkownika = '{userNameOrMail}'");
                        }
                    }
                    catch(InvalidOperationException exc)
                    {
                        ok = false;
                        alertClass = "alert-danger";
                        if (!string.IsNullOrWhiteSpace(alertValue) != false)
                            alertValue += "\n";
                        alertValue += "Nimo takiego konta, wy pikne ślonzoki.";
                    }
                    if (ok)
                    {
                        // check password
                        if (!AccountHelper.verifyPassword(password, account.HashHasla, account.LoginUzytkownika))
                        {
                            ok = false;
                            alertClass = "alert-danger";
                            if (string.IsNullOrWhiteSpace(alertValue) != false)
                                alertValue += "\n";
                            alertValue += "Złe hasło, diagnoza: alzheimer :)";
                        }
                        else
                        {
                            HttpContext.Session.SetString("loggedInAccountId", account.ID_Konta.ToString());
                            HttpContext.Session.SetString("loggedInAccountPasswordHash", account.HashHasla);
                            AccountHelper.loggedInAccount = account;
                            alertClass = "alert-success";
                            alertValue = "Zalogowanyś";
                            loggedIn = true;
                            return RedirectToPage("/Index");
                        }
                    }
                }
            }

            return Page();
        }
    }
}
