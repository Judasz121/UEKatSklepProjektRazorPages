using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Product
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        [BindProperty]
        public Produkt product { get; set; }
        public List<SelectListItem> categories { get; set; } = new List<SelectListItem>();
        [BindProperty, DataType(DataType.Upload)]
        public IFormFile productCoverPhoto { get; set; }

        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool productNotFound { get; set; }
        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    product = conn.QueryFirst<Produkt>($"SELECT TOP 1 * FROM Produkt WHERE ID_Produktu = '{id}'");
                }
            }
            catch (InvalidOperationException exc)
            {
                productNotFound = true;
            }
            this.FetchCategoriesSelectItems();
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
