using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Warehouse
{
    public class ListModel : PageModel
    {
        public IEnumerable<Magazyn> warehouse { get; set; }
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                warehouse = conn.Query<Magazyn>("SELECT * FROM Magazyn");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
