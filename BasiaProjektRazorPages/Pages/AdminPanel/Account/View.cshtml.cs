using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Account
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        public Konto account { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool accountNotFound { get; set; }
        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    account = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE ID_Konta = '{id}'");
                }
            }
            catch (InvalidOperationException exc)
            {
                accountNotFound = true;
            }
        }
    }
}
