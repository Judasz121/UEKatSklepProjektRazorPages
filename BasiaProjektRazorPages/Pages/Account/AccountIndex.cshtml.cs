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
    public class AccountIndexModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public string id { get; set; }

        [BindProperty(SupportsGet =true)]
        public string tabId { get; set; }
        public Konto account { get; set; }
        public bool accountNotFound { get; set; }
        public bool addressNotFound { get; set; }
        public bool ordersNotFound { get; set; }
        [BindProperty]
        public List<Adres> adres { get; set; }
        [BindProperty]
        public List<Zamowienie> zamowienie { get; set; }
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
                    adres = (List<Adres>)conn.Query<Adres>($"SELECT * FROM Adres WHERE ID_Klienta = '{account.ID_Klienta}'");
                    
                }
            }
            catch (InvalidOperationException exc)
            {
                addressNotFound = true;
                //this.adres = new List<Adres>();
            }


            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    zamowienie = (List<Zamowienie>)conn.Query<Zamowienie>($"SELECT * FROM Zamowienie WHERE ID_Klienta = '{account.ID_Klienta}'");
                }
            }
            catch (InvalidOperationException exc)
            {
                ordersNotFound = true;
            }
        }
        
        
    }
}
