using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using BasiaProjektRazorPages.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Warehouse
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
    
        public WarehouseViewModel warehouse { get; set; }
        public bool employeesNotFound { get; set; }
        public bool warehouseproductsNotFound { get; set; }
        public bool warehouseNotFound { get; set; }
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var temp = conn.QueryFirst<Magazyn>($"SELECT * FROM Magazyn where ID_Magazynu = @ID_Magazynu",new {ID_Magazynu = id});
                warehouse = new WarehouseViewModel(temp);
            }
        }
    }
}
