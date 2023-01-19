using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using System.Data;
using BasiaProjektRazorPages.Helpers;
using Dapper;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Product
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
