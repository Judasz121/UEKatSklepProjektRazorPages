using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel.Employee
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public string id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string redirect { get; set; }
        public Pracownik deletedEmployee { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public IActionResult OnGet()
        {
            if (AccountHelper.loggedInVerified == false)
                return Page();
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    conn.Execute($"UPDATE Pracownik SET Data_Zwolnienia = GETDATE() WHERE ID_Pracownika = @id", new { id = this.id });
                    return RedirectToPage(redirect);
                }
            }
            catch (InvalidOperationException exc)
            {
                alertClass = "alert-danger"; // check incorrect id handling
                alertMessage = "Nie znaleziono produktu z tym id lub wyst¹pi³ b³¹d serwera.";
            }

            //if (!string.IsNullOrWhiteSpace(redirect) && deletedProduct != null && deletedProduct.ID_Produktu != null)
            if (!string.IsNullOrWhiteSpace(redirect))
                return RedirectToPage(redirect);
            else
                return Page();

        }
    }
}
