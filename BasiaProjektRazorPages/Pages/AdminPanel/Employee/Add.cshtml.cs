using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class AddModel : PageModel
    {
        public IActionResult OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                int id = conn.ExecuteScalar<int>("insert into pracownik (Data_Zatrudnienia) VALUES (GETDATE()); SELECT SCOPE_IDENTITY();");

                return RedirectToPage("Edit", new { id = id });
            }
        }
    }
}
