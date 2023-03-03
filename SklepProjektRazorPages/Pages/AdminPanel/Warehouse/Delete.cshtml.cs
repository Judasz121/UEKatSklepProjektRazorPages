using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel.Warehouse
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string redirect { get; set; }
        public Magazyn deletedWarehouse { get; set; }
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
                    conn.Execute($"UPDATE Magazyn SET Usunieta = 1 WHERE ID_Magazynu = @id", new { id = this.id });
                }
            }
            catch (InvalidOperationException exc)
            {
                alertClass = "alert-danger"; // check incorrect id handling
                alertMessage = "Nie znaleziono produktu z tym id lub wyst¹pi³ se ¿ydowski b³¹d serwera.";
            }

            //if (!string.IsNullOrWhiteSpace(redirect) && deletedProduct != null && deletedProduct.ID_Produktu != null)
            if (!string.IsNullOrWhiteSpace(redirect))
                return RedirectToPage(redirect);
            else
                return Page();

        }
    }
}
