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
        public Konto account { get; set; }
        public Dostawca supplier { get; set; }
        public Klient client { get; set; }
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
        }
        [BindProperty]
        public Konto editedAccount { get; set; }
        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }
        [BindProperty]
        public bool createClient { get; set; }
        [BindProperty]
        public Klient editedClient { get; set; }
        public string clientAlertClass { get; set; }
        public string clientAlertValue { get; set; }
        [BindProperty]
        public bool createSupplier { get; set; }
        [BindProperty]
        public Dostawca editedSupplier { get; set; }
        public string supplierAlertClass { get; set; }
        public string supplierAlertValue { get; set; }


        public IActionResult OnPost()
        {
            #region Konto
            Konto oldAccount = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldAccount = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE ID_Konta = '{editedAccount.ID_Konta}'");
            }
            account = editedAccount;
            if (editedAccount != null)
            {
                account = editedAccount;
                var verification = Konto.verifyValues(editedAccount.LoginUzytkownika, editedAccount.HashHasla, editedAccount.Email);
                if (verification.Item1)
                {
                    if(editedAccount.HashHasla != null)
                    {
                        //editedAccount.HashHasla = AccountHelper.hashPassword(editedAccount.HashHasla, );
                    }
                    using (IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        //conn.Execute("UPDATE Konto SET ")
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

