using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using BasiaProjektRazorPages.ViewModels;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Delivery
{
    public class ViewModel : PageModel
    {
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool deliveryNotFound { get; set; }
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty]
        public DeliveryViewModel Delivery { get; set; }
        [BindProperty]
        public List<Produkt_magazyn_dostawa> Products_warehouses_delivery { get; set; }
        public IActionResult OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbDel = conn.Query<Dostawa>("SELECT * FROM Dostawa WHERE ID_Dostawy = @id", new { id = this.id });
                    if(dbDel.Any() == false)
                    {
                        this.deliveryNotFound = true;
                        return Page();
                    }
                    this.Delivery = new DeliveryViewModel(dbDel.First(), true);
                }
                catch (InvalidOperationException exc)
                {
                    alertClass = "alert-danger";
                    alertMessage = exc.Message;
                }
            }
            return Page();
        }
    }
}
