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
                string nullPropsSql = "(";
                int propsCount = new Produkt().GetType().GetProperties().Length - 1;
                for (int i = 0; i < propsCount; i++)
                {
                    if (i + 1 == propsCount)
                        nullPropsSql += "NULL";
                    else
                        nullPropsSql += "NULL, ";
                }
                nullPropsSql += ")";
                int id = conn.ExecuteScalar<int>("INSERT INTO Produkt VALUES" + nullPropsSql + "; SELECT SCOPE_IDENTITY();");

                return RedirectToPage("Edit", new { id = id });
            }
        }
    }
}
