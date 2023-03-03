using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel.Warehouse
{
    public class AddModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public int id { get; set; }

        [BindProperty]
        public Magazyn warehouse { get; set; }

        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }

        public IActionResult OnPost()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var verification = Magazyn.verifyValues(warehouse.Nazwa, warehouse.Kraj, warehouse.Miasto, warehouse.Kod_pocztowy, warehouse.Ulica, warehouse.Numer_budynku);
                if (verification.Item1)
                {
                    conn.Execute($"INSERT INTO Magazyn VALUES(@Nazwa,@Kraj,@Miasto,@Kod_pocztowy,@Ulica,@Numer_budynku,0)", warehouse);
                    accountAlertClass = "alert-success";
                    accountAlertValue = "Pomyœlnie dodano";
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
