using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel.Warehouse
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
