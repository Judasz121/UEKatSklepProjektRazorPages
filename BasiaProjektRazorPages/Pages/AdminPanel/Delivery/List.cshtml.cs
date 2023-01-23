using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Delivery
{
    public class ListModel : PageModel
    {
        public IEnumerable<Dostawa> delivery { get; set; }
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                delivery = conn.Query<Dostawa>("SELECT * FROM Dostawa");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
