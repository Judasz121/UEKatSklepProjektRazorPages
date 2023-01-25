using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.Account
{
    public class AccountAddAddressModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public int id { get; set; }
        [BindProperty]
        public Adres address { get; set; }

        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }
        

        public IActionResult OnPost()
        {
            using(IDbConnection conn = DbHelper.GetDbConnection())
            {
                address.changeNullStringPropertiesToEmptyStrings();
                var verification = Adres.verifyAddress(address.Kraj,address.Miasto,address.Ulica,address.Kod_pocztowy,address.Numer_budynku,address.Numer_mieszkania);
                if (verification.Item1)
                {
                    conn.Execute("INSERT INTO Adres VALUES(@ID_Klienta,@Kraj,@Miasto,@Ulica,@Kod_pocztowy,@Numer_budynku,@Numer_mieszkania)", address );
                    int clientId = conn.ExecuteScalar<int>($"SELECT ID_Konta From Konto WHERE ID_Klienta = '{address.ID_Klienta}'");
                    return RedirectToPage("AccountIndex",new { id = clientId});
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
