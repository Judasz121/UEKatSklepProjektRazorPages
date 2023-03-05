using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SklepProjektRazorPages.Pages.Product
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        public ProductViewModel product { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public List<SelectListItem> categories { get; set; } = new List<SelectListItem>();
        [BindProperty, DataType(DataType.Upload)]
        public IFormFile productCoverPhoto { get; set; }
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
                this.FetchCategoriesSelectItems();
            }
        }
        public void FetchCategoriesSelectItems()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    var dbCats = conn.Query<Kategoria>("SELECT * FROM Kategoria");
                    foreach (Kategoria cat in dbCats)
                    {
                        this.categories.Add(new SelectListItem()
                        {
                            Value = cat.ID_Kategorii.ToString(),
                            Text = cat.Nazwa
                        });
                    }
                }
            }
            catch (InvalidOperationException exc)
            {
                alertClass = "alert-danger";
                alertMessage += "Server Error: \n" + exc.Message;
            }
        }
    }
}
