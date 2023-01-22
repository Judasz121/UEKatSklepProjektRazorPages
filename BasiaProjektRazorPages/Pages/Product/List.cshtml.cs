using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using BasiaProjektRazorPages.ViewModels;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.Pages.Product
{
    public class ListModel : PageModel
    {
        public List<ProductViewModel> products { get; set; } = new List<ProductViewModel>();
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var dbProducts  = conn.Query<Produkt>($"SELECT * FROM Produkt");
                foreach(Produkt product in dbProducts)
                {
                    products.Add(new ProductViewModel(product, true));
                }
            }
        }
    }
}
