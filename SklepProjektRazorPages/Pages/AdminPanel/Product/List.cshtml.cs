using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using System.Data;
using SklepProjektRazorPages.Helpers;
using Dapper;

namespace SklepProjektRazorPages.Pages.AdminPanel.Product
{
    public class ListModel : PageModel
    {
        public IEnumerable<Produkt> products { get; set; }

        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                products = conn.Query<Produkt>("SELECT * FROM Produkt");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
