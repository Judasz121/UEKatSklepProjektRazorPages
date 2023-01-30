using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Employee
{
    public class AddModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        [BindProperty]
        public Pracownik employee { get; set; }
        public List<SelectListItem> warehouses { get; set; } = new List<SelectListItem>();


        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }

        public void OnGet()
        {
            this.FillWarehousesSelectListItems();
        }

        public IActionResult OnPost() {
            this.FillWarehousesSelectListItems();
            using (IDbConnection conn = DbHelper.GetDbConnection()) {
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

        public void FillWarehousesSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbWarehouses = conn.Query<Magazyn>("SELECT * FROM Magazyn");
                    foreach (Magazyn wh in dbWarehouses)
                    {
                        this.warehouses.Add(new SelectListItem
                        {
                            Text = wh.Nazwa,
                            Value = wh.ID_Magazynu.ToString(),
                        });
                    }
                }
                catch (InvalidOperationException exc)
                {
                    // no warehouses in db
                }
            }
        }
    }
}
