using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty]
        public Pracownik employee { get; set; }
        public List<SelectListItem> warehouses { get; set; } = new List<SelectListItem>();
        public string alertClass { get; set; }
        public string alertValue { get; set; }
        public bool emplyoeeNotFound { get; set; }
        public void OnGet()
        {
            this.FillWarehousesSelectListItems();
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
            this.FillWarehousesSelectListItems();
            Pracownik oldEmployee = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldEmployee = conn.QueryFirst<Pracownik>($"SELECT TOP 1 * FROM Pracownik WHERE ID_Pracownika = '{employee.ID_Pracownika}'");
            }
            if (!employee.Equals(oldEmployee)) {
                var verification = employee.VerifyInstanceValues(true);
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

        public void FillWarehousesSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbWarehouses = conn.Query<Magazyn>("SELECT * FROM Magazyn");
                    foreach(Magazyn wh in dbWarehouses)
                    {
                        this.warehouses.Add(new SelectListItem
                        {
                            Text = wh.Nazwa,
                            Value = wh.ID_Magazynu.ToString(),
                        });
                    }
                }
                catch(InvalidOperationException exc)
                {
                    // no warehouses in db
                }
            }
        }
    }
}
