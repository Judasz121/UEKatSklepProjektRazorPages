using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.DbModels;


namespace SklepProjektRazorPages.Pages.AdminPanel.Account
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string redirect { get; set; }
        public Konto deletedAccount { get; set; }
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
                    deletedAccount = conn.QueryFirst<Konto>($"SELECT * FROM Konto WHERE ID_Konta = '{id}'");
                    if (deletedAccount.ID_Klienta != null)
                        conn.Execute($"DELETE FROM Klient WHERE ID_Klienta = '{deletedAccount.ID_Klienta}'");
                    if (deletedAccount.ID_Dostawcy != null)
                        conn.Execute($"DELETE FROM Dostawca WHERE ID_Dostawcy = '{deletedAccount.ID_Dostawcy}' ");
                    deletedAccount = conn.QueryFirst<Konto>($"DELETE FROM Konto OUTPUT DELETED.* WHERE ID_Konta = {id}");
                }
            }
            catch(InvalidOperationException exc)
            {                
                alertClass = "alert-danger";
                alertMessage = "Nie znaleziono konta z tym id lub wyst¹pi³ se ¿ydowski b³¹d serwera.";
            }

            if (!string.IsNullOrWhiteSpace(redirect) && deletedAccount != null && deletedAccount.ID_Konta != null)
                return RedirectToPage(redirect);
            else
                return Page();
        }
    }
}
