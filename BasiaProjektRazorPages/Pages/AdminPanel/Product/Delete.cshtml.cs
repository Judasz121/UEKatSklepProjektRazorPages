using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Product
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string redirect { get; set; }
        public Produkt deletedProduct { get; set; }
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
                    try
                    {
                        conn.Execute("DELETE FROM Koszyk WHERE ID_Produktu = @id", new { id = this.id });
                    }
                    catch (InvalidOperationException exc) { }
                    deletedProduct = conn.QueryFirst<Produkt>($"DELETE FROM Produkt OUTPUT DELETED.* WHERE ID_Produktu = {id}");
                }
            }
            catch (InvalidOperationException exc)
            {
                alertClass = "alert-danger";
                alertMessage = "Nie znaleziono konta z tym id lub wyst¹pi³ se ¿ydowski b³¹d serwera.";
            }

            if (!string.IsNullOrWhiteSpace(redirect) && deletedProduct != null && deletedProduct.ID_Produktu != null)
                return RedirectToPage(redirect);
            else
                return Page();
        }
    }
}