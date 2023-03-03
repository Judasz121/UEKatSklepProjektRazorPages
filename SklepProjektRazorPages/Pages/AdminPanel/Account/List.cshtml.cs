using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using Dapper;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel
{
    public class AccountsModel : PageModel
    {
        public IEnumerable<Konto> accounts{ get; set; }

        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                accounts = conn.Query<Konto>("SELECT * FROM Konto");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
