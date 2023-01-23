using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.WarehouseProduct
{
    public class ListModel : PageModel
    {
        public IEnumerable<Produkt_magazyn> warehouseproduct { get; set; } 
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                warehouseproduct = conn.Query<Produkt_magazyn>("SELECT * FROM Produkt_magazyn");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
