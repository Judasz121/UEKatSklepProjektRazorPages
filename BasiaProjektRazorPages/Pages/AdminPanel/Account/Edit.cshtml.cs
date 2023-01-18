using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Account
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        //public Konto account { get; set; }
        //public Dostawca supplier { get; set; }
        //public Klient client { get; set; }
        public bool accountNotFound { get; set; }
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
            if (!accountNotFound)
            {
                try
                {
                    using (IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        client = conn.QueryFirst<Klient>($"SELECT TOP 1 * FROM Klient WHERE ID_Klienta = '{account.ID_Klienta}' ");
                    }
                }
                catch(Exception exc) { }
                try
                {
                    using (IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        supplier = conn.QueryFirst<Dostawca>($"SELECT TOP 1 * FROM Dostawca WHERE ID_Dostawcy = '{account.ID_Dostawcy}' ");
                    }
                }
                catch(Exception exc) { }
            }
        }
        [BindProperty]
        public Konto account { get; set; }
        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }
        [BindProperty]
        public bool createClient { get; set; }
        [BindProperty]
        public Klient client { get; set; }
        public string clientAlertClass { get; set; }
        public string clientAlertValue { get; set; }
        [BindProperty]
        public bool createSupplier { get; set; }
        [BindProperty]
        public Dostawca supplier { get; set; }
        public string supplierAlertClass { get; set; }
        public string supplierAlertValue { get; set; }


        public IActionResult OnPost()
        {
            if (!AccountHelper.loggedInVerified)
                return RedirectToPage("/Account/Login");

            
            #region Konto
            Konto oldAccount = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldAccount = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE ID_Konta = '{account.ID_Konta}'");
            }
            bool passwordChange = false;
            account.DataUtworzenia = oldAccount.DataUtworzenia;
            if (!string.IsNullOrWhiteSpace(account.HashHasla))
                passwordChange = true;
            account.HashHasla = oldAccount.HashHasla;
            bool test = !account.Equals(oldAccount); // z jakiegoœ kurwa powodu .Equals() zawsze zwraca false
            //if (account.ID_Konta != oldAccount.ID_Konta)
            //    return Page();
            //if (account.ID_Klienta != oldAccount.ID_Klienta)
            //    return Page();
            //if (account.ID_Dostawcy != oldAccount.ID_Dostawcy)
            //    return Page();
            //if (account.LoginUzytkownika != oldAccount.LoginUzytkownika)
            //    return Page();
            //if (account.Email != oldAccount.Email)
            //    return Page();
            //if (account.HashHasla != oldAccount.HashHasla)
            //    return Page();
            //if (account.JestAdminem != oldAccount.JestAdminem)
            //    return Page();
            //if (account.DataUtworzenia != oldAccount.DataUtworzenia)
            //    return Page();
            // return Page();
            if (!account.Equals(oldAccount) || passwordChange)
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

            #region Klient
            // create
            if (createClient)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    int clientId = conn.ExecuteScalar<int>("INSERT INTO Klient VALUES (NULL, NULL, NULL); SELECT SCOPE_IDENTITY();");
                    conn.Execute($"UPDATE Konto SET ID_Klienta = '{clientId}' WHERE ID_Konta = '{account.ID_Konta}'");
                    client.ID_Klienta = clientId;
                }
            }

            //edit
            if (!createClient)
            {
                Klient oldClient = null;
                bool clientNotExists = false;
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        oldClient = conn.QueryFirst<Klient>($"SELECT TOP 1 * FROM Klient WHERE ID_Klienta = '{client.ID_Klienta}'");
                    }
                    catch (InvalidOperationException exc)
                    {
                        clientNotExists = true;
                    }
                }
                bool changes1 = oldClient != client; // nulle w cliencie zwracanym z post'a + "!=" na tych 2 klasach zawsze zwraca false ( .Equals() jest te¿ zjebany )
                if (changes1 && !clientNotExists)
                {
                    Tuple<bool, string> verification = Klient.VerifyValues(client.Imie, client.Nazwisko, client.Telefon);
                    if (verification.Item1)
                    {
                        using (IDbConnection conn = DbHelper.GetDbConnection())
                        {
                            conn.Execute($"UPDATE Klient SET Imie = '{client.Imie}', Nazwisko = '{client.Nazwisko}', Telefon = '{client.Telefon}' ");
                            clientAlertClass = "alert-success";
                            clientAlertValue = "Zaktualizowano";
                        }
                    }
                    else
                    {
                        clientAlertClass = "alert-danger";
                        clientAlertValue = verification.Item2;
                    }
                }
            }
            #endregion

            #region Dostawca
            // create
            if (createSupplier)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    int supplierId = conn.ExecuteScalar<int>("INSERT INTO Dostawca VALUES (NULL, NULL, NULL, NULL); SELECT SCOPE_IDENTITY();");
                    conn.Execute($"UPDATE Konto SET ID_Dostawcy = '{supplierId}' WHERE ID_Konta = '{account.ID_Konta}'");
                    supplier.ID_Dostawcy = supplierId;
                    supplierAlertClass = "alert-success";
                    supplierAlertValue = "Dostawca utworzony";
                }
            }

            //edit
            if (!createSupplier)
            {
                Dostawca oldSupplier = null;
                bool supplierNotExists = false;
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        oldSupplier = conn.QueryFirst<Dostawca>($"SELECT TOP 1 * FROM Dostawca WHERE ID_Dostawcy = '{supplier.ID_Dostawcy}'");
                    }
                    catch (InvalidOperationException exc)
                    {
                        supplierNotExists = true;
                    }
                }
                bool changes2 = oldSupplier != supplier;
                if (changes2 && !supplierNotExists)
                {
                    Tuple<bool, string> verification = Dostawca.VerifyValues(supplier.Nazwa, supplier.Numer_telefonu, supplier.Email, supplier.NIP);
                    if (verification.Item1)
                    {
                        using (IDbConnection conn = DbHelper.GetDbConnection())
                        {
                            conn.Execute($"UPDATE Dostawca SET Nazwa = '{supplier.Nazwa}', Numer_telefonu = '{supplier.Numer_telefonu}', Email = '{supplier.Email}', NIP = '{supplier.NIP}' WHERE ID_Dostawcy = '{supplier.ID_Dostawcy}'");
                            supplierAlertClass = "alert-success";
                            supplierAlertValue = "Zaktualizowano";
                        }
                    }
                    else
                    {
                        supplierAlertClass = "alert-danger";
                        supplierAlertValue = verification.Item2;
                    }
                }
            }
            #endregion

            return Page();
        }

    }
}

