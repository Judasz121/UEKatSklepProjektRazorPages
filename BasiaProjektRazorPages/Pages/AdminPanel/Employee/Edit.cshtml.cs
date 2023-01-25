using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty]
        public Pracownik employee { get; set; }
        public string alertClass { get; set; }
        public string alertValue { get; set; }
        public bool emplyoeeNotFound { get; set; }
        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    employee = conn.QueryFirst<Pracownik>($"SELECT TOP 1 * FROM Pracownik WHERE ID_Pracownika = '{id}'");
                }
            }
            catch(Exception ex)
            {
                emplyoeeNotFound= true;
            }
        }
        public IActionResult OnPost()
        {
            //bool ok = true;
            if (!AccountHelper.loggedInVerified)
                return RedirectToPage("/Account/Login");
            Pracownik oldEmployee = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldEmployee = conn.QueryFirst<Pracownik>($"SELECT TOP 1 * FROM Pracownik WHERE ID_Pracownika = '{employee.ID_Pracownika}'");
            }
            if (!employee.Equals(oldEmployee)) {
                var verification = Pracownik.verifyValues(employee.Imie, employee.Nazwisko, employee.ID_Magazynu, employee.Wyplata, employee.Numer_telefonu,employee.PESEL, employee.Numer_konta);
                if (verification.Item1)
                {
                    using (IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        conn.Execute($"UPDATE Pracownik SET Imie = '{employee.Imie}', Nazwisko = '{employee.Nazwisko}',ID_Magazynu = '{employee.ID_Magazynu}',Wyplata = '{employee.Wyplata}',Numer_telefonu = '{employee.Numer_telefonu}', PESEL = '{employee.PESEL}', Numer_konta = '{employee.Numer_konta}' WHERE ID_Pracownika = '{employee.ID_Pracownika}'");
                    }
                    alertClass = "alert-success";
                    alertValue = "Zaktualizowano";
                }
                else
                {
                    alertClass = "alert-danger";
                    alertValue = verification.Item2;
                }
            }
            return Page();
        }
    }
}
