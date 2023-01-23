using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data;
using BasiaProjektRazorPages.Helpers;
using BasiaProjektRazorPages.DbModels;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Product
{
    public class AddModel : PageModel
    {
        public IActionResult OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                int id = conn.ExecuteScalar<int>("insert into produkt Default values; SELECT SCOPE_IDENTITY();");

                return RedirectToPage("Edit", new { id = id });
            }
        }
    }
}
