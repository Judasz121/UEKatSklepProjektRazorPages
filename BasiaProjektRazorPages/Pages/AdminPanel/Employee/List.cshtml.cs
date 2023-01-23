using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class ListModel : PageModel
    {
        public IEnumerable<Pracownik> employee { get; set; }
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                employee = conn.Query<Pracownik>("SELECT * FROM Pracownik");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
