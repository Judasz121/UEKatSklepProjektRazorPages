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
        public Adres adres { get; set; }
        [BindProperty]
        //Stworzylem liste, bo mozna byc kilka zamowien
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
                    adres = conn.QueryFirst<Adres>($"SELECT TOP 1 * FROM Adres WHERE ID_Klienta = '{id}'");
                    
                }
            }
            catch (InvalidOperationException exc)
            {
                addressNotFound = true;
            }
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
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
                ordersNotFound = true;
            }
        }
        
        
    }
}
