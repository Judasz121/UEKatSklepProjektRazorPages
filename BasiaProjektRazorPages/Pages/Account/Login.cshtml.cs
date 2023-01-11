using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Dapper;
using BasiaProjektRazorPages.DBModels;
using BasiaProjektRazorPages.Helpers;

namespace BasiaProjektRazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string userNameOrMail { get; set; }
        [BindProperty]
        public string password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
            }
            return Page();
        }
    }
}
