using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class AddModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        [BindProperty]
        public Pracownik employee { get; set; }


        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }

        public IActionResult OnPost() {
            
            using(IDbConnection conn = DbHelper.GetDbConnection()) {
                var verification = employee.VerifyInstanceValues();
                if (verification.Item1)
                {
                    conn.Execute($"INSERT INTO Pracownik VALUES(@Imie,@Nazwisko,@ID_Magazynu,@Wyplata,@Numer_telefonu,@PESEL,@Numer_konta,GETDATE(),NULL)",employee);
                    accountAlertClass = "alert-success";
                    accountAlertValue = "Dodano pomyœlnie";
                }
                else
                {
                    accountAlertClass = "alert-danger";
                    accountAlertValue = verification.Item2;
                }
            }


            return Page();
        }
    }
}
