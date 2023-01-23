using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        public Pracownik employee { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool employeeNotFound { get; set; }
        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    employee = conn.QueryFirst<Pracownik>($"SELECT TOP 1 * FROM Pracownik WHERE ID_Pracownika = '{id}'");
                }
            }
            catch(InvalidOperationException exc)
            {
                employeeNotFound = true;   
            }
        }
    }
}
