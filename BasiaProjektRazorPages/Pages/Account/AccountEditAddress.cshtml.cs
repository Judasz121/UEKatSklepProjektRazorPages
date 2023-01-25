using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.Account
{
    public class AccountEditAddressModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        public bool addressNotFound { get; set; }
        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }
        [BindProperty]
        public Adres address { get; set; }
        public void OnGet()
        {
            try
            {
                using(IDbConnection conn = DbHelper.GetDbConnection())
                {
                    address = conn.QueryFirst<Adres>($"SELECT TOP 1 * FROM Adres WHERE ID_Adresu = '{id}'");
                }
            }
            catch
            {
                addressNotFound = true;
            }
        }

        public IActionResult OnPost()
        {
            Adres oldAddress = null;
            bool addressDoesntExist = false;

            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    oldAddress = conn.QueryFirst<Adres>($"SELECT TOP 1 * FROM Adres WHERE ID_Klienta = '{address.ID_Klienta}'");
                }
                catch (InvalidOperationException exc)
                {
                    addressDoesntExist= true;
                }
            }

            if (!address.Equals(oldAddress))
            {
                address.changeNullStringPropertiesToEmptyStrings();
                var verification = Adres.verifyAddress(address.Kraj, address.Miasto, address.Ulica, address.Kod_pocztowy, address.Numer_budynku, address.Numer_mieszkania);
                if (verification.Item1)
                {
                    using(IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        conn.Execute($"UPDATE Adres SET Kraj = '{address.Kraj}',Miasto = '{address.Miasto}',Ulica = '{address.Ulica}', Kod_pocztowy = '{address.Kod_pocztowy}', Numer_budynku = '{address.Numer_budynku}', Numer_mieszkania = '{address.Numer_mieszkania}' WHERE ID_Adresu = '{address.ID_Adresu}'");
                    }
                    accountAlertClass = "alert-success";
                    accountAlertValue = "Zaktualizowano";
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
