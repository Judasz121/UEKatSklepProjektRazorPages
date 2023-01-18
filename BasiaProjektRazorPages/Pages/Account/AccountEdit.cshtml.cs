using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Principal;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BasiaProjektRazorPages.Pages.Account
{
    public class AccountEditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        public bool accountNotFound { get; set; }
        public bool addressNotFound { get; set; }

        [BindProperty]
        public List<Zamowienia> zamowienie { get; set; }
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
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    address = conn.QueryFirst<Adres>($"SELECT TOP 1 * FROM Adres WHERE ID_Klienta = '{id}'");
                    //Podjeba³em z neta rozwi¹zanie 
                    // https://www.aspsnippets.com/Articles/Using-SqlDataReader-in-ASPNet-Core-Razor-Pages.aspx
                    //Jak da sie skróciæ kod, bo nie wiem jak dzia³a dok³adnie po³¹czenie z baz¹ to ogarnij thx :)
                    string query_z = $"Select * FROM Zamowienie WHERE ID_Klienta = '{id}'";
                    using (SqlCommand cmd = new SqlCommand(query_z, (SqlConnection)conn))
                    {
                        conn.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            zamowienie = new List<Zamowienia>();
                            while (sdr.Read())
                            {
                                zamowienie.Add(new Zamowienia
                                {
                                    ID_Zamowienia = int.Parse(sdr["ID_Zamowienia"].ToString()),
                                    Data_zamowienia = DateTime.Parse(sdr["Data_Zamowienia"].ToString()),
                                    Zaplacone = bool.Parse(sdr["Zaplacone"].ToString())

                                });
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (InvalidOperationException exc)
            {
                addressNotFound = true;
            }

        }
        [BindProperty]
        public Konto account { get; set; }
        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }
        [BindProperty]
        public Adres address { get; set; }
        public IActionResult OnPost()
        {

            #region Konto
            Konto oldAccount = null;
            Adres oldAdress = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldAccount = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE ID_Konta = '{account.ID_Konta}'");
                oldAdress = conn.QueryFirst<Adres>($"SELECT TOP 1 * FROM Adres WHERE ID_Konta = '{address.ID_Klienta}'");
            }
            if (account != null)
            {
                var verification = Konto.verifyValues(account.LoginUzytkownika, account.HashHasla, account.Email);
                if (verification.Item1)
                {
                    if (account.HashHasla != null)
                    {
                        account.HashHasla = AccountHelper.hashPassword(account.HashHasla, "ojciec");
                    }
                    else
                        account.HashHasla = oldAccount.HashHasla;
                    using (IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        conn.Execute($"UPDATE Konto SET LoginUzytkownika = '{account.LoginUzytkownika}', Email = '{account.Email}', JestAdminem = '{account.JestAdminem}'");
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

            #endregion

            return Page();
        }
    }
}
