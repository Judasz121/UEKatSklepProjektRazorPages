using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.ViewModels;

namespace SklepProjektRazorPages.Pages.Product
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        public ProductViewModel product { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    Produkt dbProd = conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @id", new { id = this.id });
                    product = new ProductViewModel(dbProd, true);
                }
                catch(InvalidOperationException exc)
                {
                    alertMessage = exc.Message;
                    alertClass = "alert-danger";
                }
            }
        }
    }
}
