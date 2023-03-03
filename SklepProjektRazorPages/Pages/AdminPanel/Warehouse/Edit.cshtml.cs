using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel.Warehouse
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty]
        public Magazyn warehouse { get; set; }
        public string alertClass { get; set; }
        public string alertValue { get; set; }
        public bool warehouseNotFound { get; set; }
        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    warehouse = conn.QueryFirst<Magazyn>($"SELECT TOP 1 * FROM Magazyn WHERE ID_Magazynu = '{id}'");
                }
            }
            catch (Exception ex)
            {
                warehouseNotFound = true;
            }
        }
        public IActionResult OnPost()
        {
            if (!AccountHelper.loggedInVerified)
                return RedirectToPage("/Account/Login");
            Magazyn oldWarehouse = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldWarehouse = conn.QueryFirst<Magazyn>($"SELECT TOP 1 * FROM Magazyn WHERE ID_Magazynu = '{warehouse.ID_Magazynu}'");
            }
            if(!warehouse.Equals(oldWarehouse)) {
                var verification = Magazyn.verifyValues(warehouse.Nazwa, warehouse.Kraj, warehouse.Miasto, warehouse.Kod_pocztowy, warehouse.Ulica, warehouse.Numer_budynku);
                if (verification.Item1)
                {
                    using(IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        conn.Execute($"UPDATE Magazyn SET Nazwa = '{warehouse.Nazwa}', Kraj = '{warehouse.Kraj}', Miasto = '{warehouse.Miasto}', Kod_pocztowy = '{warehouse.Kod_pocztowy}', Ulica = '{warehouse.Ulica}', Numer_budynku = '{warehouse.Numer_budynku}' WHERE ID_Magazynu = '{warehouse.ID_Magazynu}'");
                    }
                    alertClass = "alert-success";
                    alertValue = "Zaktualizowano";
                }
                else
                {
                    alertClass = "alter-danger";
                    alertValue = verification.Item2;
                }
                
            }
            return Page();
        }
    }
}
