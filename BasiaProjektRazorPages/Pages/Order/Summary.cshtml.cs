using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using BasiaProjektRazorPages.ViewModels;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace BasiaProjektRazorPages.Pages.Order
{
    public class SummaryModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? id { get; set; }
        public OrderViewModel order { get; set; }
        public List<SelectListItem> addresses { get; set; } = new List<SelectListItem>();
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool boolek { get; set; }
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                Zamowienie dbOrder = null;
                if (this.id == null)
                {
                    dbOrder = conn.QueryFirst<Zamowienie>("SELECT * FROM Zamowienie WHERE ID_Klienta = @ID_Klienta AND Zlozone = 0",
                        new
                        {
                            ID_Klienta = AccountHelper.loggedInAccount.ID_Klienta
                        });
                }
                else
                {
                    dbOrder = conn.QueryFirst<Zamowienie>("SELECT * FROM Zamowienie WHERE ID_Zamowienia = @ID_Zamowienia",
                        new
                        {
                            ID_Zamowienia = dbOrder.ID_Zamowienia
                        });
                }
                var adresy = conn.Query<Adres>("SELECT * FROM Adres WHERE ID_Klienta = @ID_Klienta", new
                {
                    ID_Klienta = AccountHelper.loggedInAccount.ID_Klienta
                });
                foreach(Adres adres in adresy)
                {
                    adres.changeNullStringPropertiesToEmptyStrings();
                    this.addresses.Add(new SelectListItem() { Text = adres.Kraj + adres.Miasto + adres.Ulica + adres.Kod_pocztowy + adres.Numer_budynku + adres.Numer_mieszkania, Value = adres.ID_Adresu.ToString() });
                }
                this.order = new OrderViewModel(dbOrder);
            }
        }

        public void OnPost()
        {
            // zmien adres btn submit
            if (Request.Form.ContainsKey("changeAddress"))
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    conn.Execute("UPDATE Zamowienie SET ID_Adresu = @ID_Adresu WHERE ID_Zamowienia = @ID_Zamowienia",
                        new { ID_Adresu = order.ID_Adresu, ID_Zamowienia = order.ID_Zamowienia });
                }
            }
            else
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    conn.Execute("UPDATE Zamowienie SET ID_Adresu = @ID_Adresu, Data_Zamowienia = GETDATE(), Zlozone = 1 WHERE ID_Zamowienia = @ID_Zamowienia",
                        new { ID_Adresu = order.ID_Adresu, ID_Zamowienia = order.ID_Zamowienia });
                }
            }
        }

        public void fillAddressesProperty(int? clientId = null)
        {
            if(clientId == null)
                clientId = AccountHelper.loggedInAccount.ID_Klienta;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var dbAddresses = conn.Query<Adres>("SELECT * FROM Adres WHERE ID_Klienta = @ID_Klienta", new { ID_Klienta = clientId });
                foreach(Adres a in dbAddresses)
                {
                    var props = a.GetType().GetProperties();
                    // make null string props empty strings
                    foreach(PropertyInfo prop in props)
                    {
                        // is of type string and is null
                        if (typeof(string).IsAssignableFrom(prop.GetType()) && prop.GetValue(a) == null)
                        {
                            prop.SetValue(a, string.Empty);
                        }
                    }
                    this.addresses.Add(new SelectListItem
                    {
                        Value = a.ID_Adresu.ToString(),
                        Text = $"{a.Kraj} {a.Miasto} {a.Ulica} {a.Kod_pocztowy} {a.Numer_budynku} {a.Numer_mieszkania}"
                    });
                }
            }
        }
    }
}
