using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.ViewModels;
using System.Data;
using Dapper;

namespace SklepProjektRazorPages.Pages.Product
{
    public class ListModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public string searching_bar { get;set; }
        public List<ProductViewModel> products { get; set; } = new List<ProductViewModel>();
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var dbProducts  = conn.Query<Produkt>($"SELECT TOP 10 * FROM Produkt where usuniety = 0 and Nazwa like '%{searching_bar}%'");
                foreach(Produkt product in dbProducts)
                {
                    products.Add(new ProductViewModel(product, true));
                }     
            }
        }
    }
}
